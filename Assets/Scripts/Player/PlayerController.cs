using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    private float gravity = -40f;
    [SerializeField] private float fallMultiplier = 2f;

    [Header("Collisions")]
    [SerializeField] private LayerMask collideWith;
    [SerializeField] private int verticalRayAmount = 4;
    [SerializeField] private int horizontalRayAmount = 4;


    private MovingPlatform _movingPlatform;


    // Return if the Player is facing Right
    public bool FacingRight { get; set; }

    // Return the Gravity value
    public float Gravity => gravity;

    // Return the Force applied 
    public Vector2 Force => _force;

    // Return the conditions
    public PlayerConditions Conditions => _conditions;

    public float Friction { get; set; }

    private BoxCollider2D _boxCollider2D;
    private PlayerConditions _conditions;

    private Vector2 _boundsTopLeft;
    private Vector2 _boundsTopRight;
    private Vector2 _boundsBottomLeft;
    private Vector2 _boundsBottomRight;

    private float _boundsWidth;
    private float _boundsHeight;

    private float _currentGravity;
    private Vector2 _force;
    private Vector2 _movePosition;

    private float _skin = 0.05f;

    private float _internalFaceDirection = 1f;
    private float _faceDirection;

    private float _wallFallMultiplier;

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _conditions = new PlayerConditions();
        _conditions.Reset();

    }

    private void Update()
    {
        ApplyGravity();
        StartMovement();

        EnterPlatformMovement();
        SetRayOrigins();
        GetFaceDirection();
        RotateModel();
        if (FacingRight)
        {
            HorizontalCollision(1);
        }
        else
        {
            HorizontalCollision(-1);
        }
        CollisionBelow();
        CollisionAbove();



        /*
        Debug.DrawRay(_boundsBottomLeft, Vector2.left, Color.green);
        Debug.DrawRay(_boundsBottomRight, Vector2.right, Color.green);
        Debug.DrawRay(_boundsTopLeft, Vector2.left, Color.green);
        Debug.DrawRay(_boundsTopRight, Vector2.right, Color.green);
        */
        if(!GetComponent<SpriteRenderer>().enabled)
        {
            _force = Vector2.zero;
            _movePosition = Vector2.zero;
        }
        transform.Translate(_movePosition, Space.Self);
        SetRayOrigins();
        CalculateMovement();

    }

    // Calculate ray based on our collider
    private void SetRayOrigins()
    {
        Bounds playerBounds = _boxCollider2D.bounds;

        _boundsBottomLeft = new Vector2(playerBounds.min.x, playerBounds.min.y);
        _boundsBottomRight = new Vector2(playerBounds.max.x, playerBounds.min.y);
        _boundsTopLeft = new Vector2(playerBounds.min.x, playerBounds.max.y);
        _boundsTopRight = new Vector2(playerBounds.max.x, playerBounds.max.y);

        _boundsHeight = Vector2.Distance(_boundsBottomLeft, _boundsTopLeft);
        _boundsWidth = Vector2.Distance(_boundsBottomLeft, _boundsBottomRight);
    }

    // Sets our new x movement
    public void SetHorizontalForce(float xForce)
    {
        _force.x = xForce;
    }

    public void SetVerticalForce(float yForce)
    {
        _force.y = yForce;
    }



    private void ApplyGravity()
    {
        _currentGravity = Conditions.currentGravity;
        if (_currentGravity != 0)
        {
            _force.y += _currentGravity * Time.deltaTime;
            if (_force.y < 0)
            {
                _currentGravity *= fallMultiplier;
            }
            if (_wallFallMultiplier != 0)
            {
                _force.y *= _wallFallMultiplier;
            }
        }
        else
            _force.y = 0;
    }

    public void SetWallClingMultiplier(float fallM)
    {
        _wallFallMultiplier = fallM;
    }

    private void StartMovement()
    {
        _conditions.Reset();
        _movePosition = _force * Time.deltaTime;
    }

    private void CollisionBelow()
    {
        Friction = 0f;
        if (_movePosition.y < -0.0001f)
        {
            _conditions.IsFalling = true;
        }
        else
        {
            _conditions.IsFalling = false;
        }

        if (!_conditions.IsFalling)
        {
            _conditions.IsCollidingBelow = false;
            return;  // if the Player going UP, then return because no point to calculate other colliding below.
        }

        // Calculate ray length
        float rayLenght = _boundsHeight / 2f + _skin;
        if (_movePosition.y < 0)
        {
            rayLenght += Mathf.Abs(_movePosition.y);
        }

        // Calculate ray origin
        Vector2 leftOrigin = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rightOrigin = (_boundsBottomRight + _boundsTopRight) / 2f;
        leftOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);
        rightOrigin += (Vector2)(transform.up * _skin) + (Vector2)(transform.right * _movePosition.x);

        // Raycast
        for (int i = 0; i < verticalRayAmount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(leftOrigin, rightOrigin, (float)i / (float)(verticalRayAmount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -transform.up, rayLenght, collideWith);
            Debug.DrawRay(rayOrigin, -transform.up * rayLenght, Color.green);
            if (hit)
            {
                GameObject hitObject = hit.collider.gameObject;
                if (_force.y > 0)
                {
                    _movePosition.y = _force.y * Time.deltaTime;
                    _conditions.IsCollidingBelow = false;
                }
                else
                {
                    _movePosition.y = -hit.distance + _boundsHeight / 2f + _skin;
                }

                _conditions.IsCollidingBelow = true;
                _conditions.IsFalling = false;

                if (Mathf.Abs(_movePosition.y) < 0.0001f)
                {
                    _movePosition.y = 0f;
                }

                if (hitObject.GetComponent<SpecialSurface>() != null)
                {
                    Friction = hitObject.GetComponent<SpecialSurface>().Friction;
                }

                
                if (hitObject.GetComponent<MovingPlatform>() != null)
                {
                    _movingPlatform = hitObject.GetComponent<MovingPlatform>();
                }


            }
            else
            {
                _conditions.IsCollidingBelow = false;
            }

        }

    }

    private void CollisionAbove()
    {
        if (_movePosition.y < 0)
        {
            return;
        }

        // Set rayLenght
        float rayLenght = _movePosition.y + _boundsHeight / 2f;

        // Origin Points
        Vector2 rayTopLeft = (_boundsBottomLeft + _boundsTopLeft) / 2f;
        Vector2 rayTopRight = (_boundsBottomRight + _boundsTopRight) / 2f;
        rayTopLeft += (Vector2)transform.right * _movePosition.x;
        rayTopRight += (Vector2)transform.right * _movePosition.x;

        for (int i = 0; i < verticalRayAmount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayTopLeft, rayTopRight, (float)i / (float)(verticalRayAmount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, transform.up, rayLenght, collideWith);
            Debug.DrawRay(rayOrigin, transform.up * rayLenght, Color.red);

            if (hit)
            {
                _movePosition.y = hit.distance - _boundsHeight / 2f;
                _conditions.IsCollidingAbove = true;
            }
        }
    }

    private void HorizontalCollision(int direction)
    {
        Vector2 rayHorizontalBottom = (_boundsBottomLeft + _boundsBottomRight) / 2f;
        Vector2 rayHorizontalTop = (_boundsTopLeft + _boundsTopRight) / 2f;
        rayHorizontalBottom += (Vector2)transform.up * _skin;
        rayHorizontalTop -= (Vector2)transform.up * _skin;

        float rayLenght = Mathf.Abs(_force.x * Time.deltaTime) + _boundsWidth / 2f + _skin * 2f;

        for (int i = 0; i < horizontalRayAmount; i++)
        {
            Vector2 rayOrigin = Vector2.Lerp(rayHorizontalBottom, rayHorizontalTop, (float)i / (horizontalRayAmount - 1));
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction * transform.right, rayLenght, collideWith);
            Debug.DrawRay(rayOrigin, transform.right * rayLenght * direction, Color.cyan);

            if (hit)
            {
                if (direction >= 0)
                {
                    _movePosition.x = hit.distance - _boundsWidth / 2f - _skin * 2f;
                    _conditions.IsCollidingRight = true;
                }
                else
                {
                    _movePosition.x = -hit.distance + _boundsWidth / 2f + _skin * 2f;
                    _conditions.IsCollidingLeft = true;
                }

                _force.x = 0f;
            }
        }
    }

    private void EnterPlatformMovement()
    {
        if (_movingPlatform == null)
        {
            return;
        }

        if (_movingPlatform.CollidingWithPlayer)
        {
            if (_movingPlatform.MoveSpeed != 0)
            {
                Vector3 moveDirection = _movingPlatform.Direction == PathFollow.MoveDirections.RIGHT
                    ? Vector3.right
                    : Vector3.left;
                transform.Translate(moveDirection * _movingPlatform.MoveSpeed * Time.deltaTime);
            }
        }
    }


    private void CalculateMovement()
    {
        if (Time.deltaTime > 0)
        {
            _force = _movePosition / Time.deltaTime;
        }
    }
    // Manage the direction we are facing
    private void GetFaceDirection()
    {
        _faceDirection = _internalFaceDirection;
        FacingRight = _faceDirection == 1;  // if FacingRight is TRUE

        if (_force.x > 0.0001f)
        {
            _faceDirection = 1f;
            FacingRight = true;
        }
        else if (_force.x < -0.0001f)
        {
            _faceDirection = -1f;
            FacingRight = false;
        }

        _internalFaceDirection = _faceDirection;
    }

    private void RotateModel()
    {
        if (FacingRight)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }


}