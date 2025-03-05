using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJump : Collectable
{
    [SerializeField] AudioClip jumpPickUpSE;
    
    protected override void Collect()
    {
        AddJump();
        _audioSource.PlayOneShot(jumpPickUpSE);
    }

    protected override bool CanBePicked()
    {

        if (_playerMotor != null && _playerMotor.GetComponent<PlayerJump>() != null)
        {
            var playerJump = _playerMotor.GetComponent<PlayerJump>();
            if (playerJump.JumpsLeft == playerJump.maxJumps)
            {
                return false;
            }
        }
        return base.CanBePicked();
    }

    // Adds jump
    private void AddJump()
    {
        if (_playerMotor.GetComponent<PlayerJump>() == null)
        {
            return;
        }

        var playerJump = _playerMotor.GetComponent<PlayerJump>();
        if (playerJump.JumpsLeft < playerJump.maxJumps)
        {
            playerJump.JumpsLeft += 1;
        }
    }

}
