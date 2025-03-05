using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static Action<PlayerMotor> OnPlayerSpawn;

    [Header("Settings")]
    [SerializeField] private Transform levelStartPoint;
    [SerializeField] private GameObject player;
    private Transform currentSpawnPoint;

    private PlayerMotor _currentPlayer;

    private void Awake()
    {
        currentSpawnPoint = levelStartPoint;
        SpawnPlayer(player);
    }



    private void Update()
    {

    }

    // Spawns our player in the spawnPoint   
    private void SpawnPlayer(GameObject player)
    {
        if (player != null)
        {
            _currentPlayer = player.GetComponent<PlayerMotor>();
            _currentPlayer.SpawnPlayer(levelStartPoint);
        }
    }

    // Revives our player
    private void RevivePlayer()
    {
        if (_currentPlayer != null)
        {
            _currentPlayer.GetComponent<SpriteRenderer>().enabled = true;
            _currentPlayer.GetComponent<BoxCollider2D>().enabled = true;
            _currentPlayer.SpawnPlayer(currentSpawnPoint);
        }
    }

    private void PlayerDeath(PlayerMotor player)
    {
        _currentPlayer.GetComponent<SpriteRenderer>().enabled = false;
        _currentPlayer.GetComponent<BoxCollider2D>().enabled = false;
        StartCoroutine("WaitForRevive");

    }

    private void OnEnable()
    {
        Health.OnDeath += PlayerDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= PlayerDeath;
    }

    IEnumerator WaitForRevive()
    {
        yield return new WaitForSeconds(0.5f);
        RevivePlayer();
    }

    public void SetSpawnPoint(Transform newSpawnPoint)
    {
        currentSpawnPoint = newSpawnPoint;
    }
}