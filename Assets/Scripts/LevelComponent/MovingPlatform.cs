using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : PathFollow
{
    public bool CollidingWithPlayer { get; set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            CollidingWithPlayer = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        CollidingWithPlayer = false;
    }
}