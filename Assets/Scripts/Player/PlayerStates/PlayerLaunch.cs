using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaunch : PlayerStates
{
    [SerializeField] private GameObject crossHair;
    public bool isTouchingGrip;
    private Vector3 gripPos;
    private bool isGrabbing;
    private bool isLaunched;
    [SerializeField] private float maxGrabTime = 1.5f;
    private float currentGrabTime;
    private float gravity;
    [SerializeField] private float dashSpeed = 25;
    [SerializeField] private float dashTime = 0.5f;
    private float dashTimeCount;
    private Vector2 direction;
    [SerializeField] private AudioClip launchSE;
    [SerializeField] private AudioClip gripSE;

    protected override void InitState()
    {
        base.InitState();
        isTouchingGrip = false;
        isGrabbing = false;
        currentGrabTime = 0;
        dashTimeCount = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void ExecuteState()
    {
        Grab();
        Launch();
        crossHair.SetActive(isGrabbing);
        if(isGrabbing&& Cursor.lockState != CursorLockMode.Confined)
            Cursor.lockState = CursorLockMode.Confined;
    }

    private void Launch()
    {
        if (isLaunched && dashTimeCount < dashTime)
        {
            dashTimeCount += Time.deltaTime;
            _playerController.SetHorizontalForce(dashSpeed * direction.x);
            _playerController.SetVerticalForce(dashSpeed * direction.y);
        }
        else if (dashTimeCount >= dashTime)
        {
            dashTimeCount = 0;
            _playerController.SetVerticalForce(0);
            isLaunched = false;
        }
        else if (isLaunched &&
            (_playerController.Conditions.IsCollidingLeft || _playerController.Conditions.IsCollidingRight))
        {
            _playerController.SetVerticalForce(0);
            isLaunched = false;
        }

    }
    private void Grab()
    {
        if (isGrabbing)
        {
            gravity = _playerController.Conditions.gravity;
            _playerController.Conditions.currentGravity = 0;
            transform.position = gripPos;
            direction = crossHair.transform.position - this.transform.position;
            direction = direction.normalized;
            currentGrabTime += Time.deltaTime;
            
            if (currentGrabTime >= maxGrabTime)
            {
                ResetGrab();
                isLaunched = true;
                _audioSource.PlayOneShot(launchSE);
            }
        }
    }

    private void ResetGrab()
    {
        _playerController.Conditions.currentGravity = gravity;
        isGrabbing = false;
        currentGrabTime = 0;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Grip")
        {
            gripPos = collision.transform.position;
            isTouchingGrip = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Grip")
        {
            isTouchingGrip = false;
        }
    }

    protected override void GetInput()
    {
        if (Input.GetMouseButtonDown(0) && isTouchingGrip)
        {
            isGrabbing = true;
            _audioSource.PlayOneShot(gripSE);
        }
        if (!isLaunched)
        {
            if (Input.GetMouseButton(0) && isTouchingGrip)
                isGrabbing = true;
        }
        
        if (Input.GetMouseButtonUp(0) && isTouchingGrip)
        {
            ResetGrab();
            _audioSource.PlayOneShot(launchSE);
            isLaunched = true;
        }
    }

    private void PlayerDeath(PlayerMotor player)
    {
        ResetGrab();
        isLaunched = false;
        dashTimeCount = 0;
    }

    private void OnEnable()
    {
        Health.OnDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= PlayerDeath;
    }
}
