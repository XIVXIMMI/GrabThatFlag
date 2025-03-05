using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDash : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float dashSpeed;
    [SerializeField] public int maxDashCount = 1;
    [SerializeField] private float dashTime = 0.5f;
    [SerializeField] private AudioClip dashSE;
    public int dashLeft;
    private int direction = 0;
    private float dashTimeCount = 0;

    private int _dashAnimatorParameter = Animator.StringToHash("Dash");

    protected override void InitState()
    {
        base.InitState();
        dashLeft = maxDashCount;
    }

    public override void ExecuteState()
    {
        Dash();
        if (_playerController.Conditions.IsCollidingBelow && _playerController.Force.y == 0f)
        {
            dashLeft = maxDashCount;
        }
    }

    // to move the player
    private void Dash()
    {
        if (_playerController.Conditions.IsDashing && dashTimeCount < dashTime)
        {
            dashTimeCount += Time.deltaTime;
            _playerController.SetHorizontalForce(dashSpeed * direction);
            _playerController.Conditions.IsDashing = true;
            _playerController.Conditions.IsFalling = false;
            _playerController.Conditions.IsCollidingBelow = false;
        }
        else if (dashTimeCount >= dashTime)
        {
            dashTimeCount = 0;
            _playerController.Conditions.IsDashing = false;
        }

        else if (_playerController.Conditions.IsDashing &&
            (_playerController.Conditions.IsCollidingLeft || _playerController.Conditions.IsCollidingRight))
        {
            dashTimeCount = 0;
            _playerController.Conditions.IsDashing = false;
        }
    }

    protected override void GetInput()
    {
        if (Input.GetMouseButtonDown(1)&&CanDash())
        {
            if (_playerController.FacingRight)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }
            dashLeft--;
            _playerController.Conditions.IsDashing = true;
            _audioSource.PlayOneShot(dashSE);
        }
    }

    //Dash animation
    public override void SetAnimation()
    {
        _animator.SetBool(_dashAnimatorParameter, _playerController.Conditions.IsDashing);      //Works in air, but on ground it was interupted by idle animation
    }
    private bool CanDash()
    {
        if (!_playerController.Conditions.IsCollidingBelow && dashLeft <= 0)
        {
            return false;
        }

        if (_playerController.Conditions.IsCollidingBelow && dashLeft <= 0)
        {
            return false;
        }

        return true;
    }
    private void PlayerDeath(PlayerMotor player)
    {
        dashLeft = maxDashCount;
        dashTimeCount = 0;
        _playerController.Conditions.IsDashing = false;
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