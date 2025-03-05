using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Action<PlayerMotor> OnDeath;
    public static Action<PlayerMotor> OnRevive;
    
    private AudioSource audioSource;

    [Header("Settings")]
    [SerializeField] private AudioClip damageSE;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    public void KillPlayer()
    {
        audioSource.PlayOneShot(damageSE);
        OnDeath?.Invoke(gameObject.GetComponent<PlayerMotor>());
    }

    public void Revive()
    {
        OnRevive?.Invoke(gameObject.GetComponent<PlayerMotor>());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().Damage(gameObject.GetComponent<PlayerMotor>());
        }
    }
}
