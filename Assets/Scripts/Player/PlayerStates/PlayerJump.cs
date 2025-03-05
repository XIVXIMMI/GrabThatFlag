using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : PlayerStates
{
    [Header("Settings")]
    [SerializeField] private float maxJumpUnit = 4;//Number of max units reached by jump
    [SerializeField] private float minJumpUnit = 1;//Number of min units reached by jump
    [SerializeField] private float timeToJumpApex = .4f;//Time to reach max height(Lower this value, bigger gravity)
    [SerializeField] public int maxJumps = 2;
    [SerializeField] private AudioClip jumpSE;
    private float maxJumpVelocity;
    private float minJumpVelocity;

    private int _jumpAnimatorParameter = Animator.StringToHash("Jump");
    private int _doubleJumpParameter = Animator.StringToHash("DoubleJump");
    private int _fallAnimatorParameter = Animator.StringToHash("Fall");

    private bool isPause;

    public int JumpsLeft { get; set; }

    float countAfterPause = 0;

    protected override void InitState()
    {
        base.InitState();
        isPause = Time.timeScale == 0 ? true : false;
        JumpsLeft = maxJumps;
        _playerController.Conditions.gravity = -(2 * maxJumpUnit) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(_playerController.Conditions.gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(_playerController.Conditions.gravity) * minJumpUnit);//v^2 - v0^2 = 2ax��v0 = 0�Ƃ����Ƃ��̎��ϊ�
        Debug.Log("Gravity: " + _playerController.Conditions.gravity + ", Max Jump Velocity: " + maxJumpVelocity + ", Min Jump Velocity: " + minJumpVelocity);
    }

    public override void ExecuteState()
    {
        if (_playerController.Conditions.IsCollidingBelow && _playerController.Force.y == 0f)
        {
            JumpsLeft = maxJumps;
            _playerController.Conditions.IsJumping = false;
        }
        if((_playerController.Conditions.IsCollidingLeft||_playerController.Conditions.IsCollidingRight)
            && !_playerController.Conditions.IsCollidingBelow)
        {
            JumpsLeft = 1;
        }    
    }


    protected override void GetInput()
    {
        bool lastIsPause = isPause;
        isPause = Time.timeScale == 0 ? true : false;
        if (isPause)
            return;
        if (!isPause && lastIsPause)
        {
            countAfterPause += Time.deltaTime;
            if (countAfterPause < 0.2f)
                return;
            else
                countAfterPause = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInptDown();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpInputUp();
        }
    }

    private bool CanJump()
    {
        if (!_playerController.Conditions.IsCollidingBelow && JumpsLeft <= 0)
        {
            return false;
        }

        if (_playerController.Conditions.IsCollidingBelow && JumpsLeft <= 0)
        {
            return false;
        }

        return true;
    }

    protected virtual void OnJumpInputUp()
    {
        if (minJumpVelocity < _playerController.Force.y && _playerController.Conditions.IsJumping)
        {
            _playerController.SetVerticalForce(minJumpVelocity);
        }
    }

    protected virtual void OnJumpInptDown()
    {
        if (CanJump()&&JumpsLeft != 0)
        {
            _playerController.SetVerticalForce(maxJumpVelocity);
            JumpsLeft -= 1;
            _playerController.Conditions.IsJumping = true;
            _audioSource.PlayOneShot(jumpSE);
            //Debug.Log(JumpsLeft);
        }
    }

    public override void SetAnimation()
    {
        // Jump
        _animator.SetBool(_jumpAnimatorParameter, _playerController.Conditions.IsJumping
                                                  && !_playerController.Conditions.IsCollidingBelow
                                                  && JumpsLeft > 0
                                                  && !_playerController.Conditions.IsFalling);

        // Double jump
        _animator.SetBool(_doubleJumpParameter, _playerController.Conditions.IsJumping
                                                  && !_playerController.Conditions.IsCollidingBelow
                                                  && JumpsLeft == 0
                                                  && !_playerController.Conditions.IsFalling);


        // Fall
        _animator.SetBool(_fallAnimatorParameter, _playerController.Conditions.IsFalling
                                                  && _playerController.Conditions.IsJumping
                                                  && !_playerController.Conditions.IsCollidingBelow
                                                  && !_animator.GetBool("WallSlide"));
                                          
    }


    private void JumpResponse(float jump)
    {
        _playerController.SetVerticalForce(Mathf.Sqrt(2f * jump * Mathf.Abs(_playerController.Gravity)));
    }

}

