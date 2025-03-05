using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Camera2DShake : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float shakeVibrato = 10f;
    [SerializeField] private float shakeRandomness = 0.2f;
    [SerializeField] private float shakeTime = 0.01f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Shake();
        }
    }

    // Call the shake logic
    public void Shake()
    {
        StartCoroutine(IEShake());
    }

    // Shakes the camera position
    private IEnumerator IEShake()
    {
        Vector3 currentPos = transform.position;
        for (int i = 0; i < shakeVibrato; i++)
        {
            Vector3 shakePos = currentPos + Random.onUnitSphere * shakeRandomness;
            yield return new WaitForSeconds(shakeTime);
            transform.position = shakePos;
        }
    }
}