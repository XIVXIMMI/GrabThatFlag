using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CDash : Collectable
{
    [SerializeField] private AudioClip dachPickUpSE;

    protected override void Collect()
    {
        AddDash();
    }

    protected override bool CanBePicked()
    {

        if (_playerMotor != null&&_playerMotor.GetComponent<PlayerDash>() != null)
        {
            var playerDash = _playerMotor.GetComponent<PlayerDash>();
            if (playerDash.dashLeft == playerDash.maxDashCount)
            {
                return false;
            }
        }
        return base.CanBePicked();
    }

    // Adds life
    private void AddDash()
    {
        _audioSource.PlayOneShot(dachPickUpSE);

        if (_playerMotor.GetComponent<PlayerDash>() == null)
        {
            return;
        }

        var playerDash = _playerMotor.GetComponent<PlayerDash>();
        if (playerDash.dashLeft < playerDash.maxDashCount)
        {
            playerDash.dashLeft += 1;
        }
    }
}
