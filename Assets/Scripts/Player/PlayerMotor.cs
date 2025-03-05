using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private PlayerStates[] _playerStates;

    // Start is called before the first frame update
    private void Start()
    {
        _playerStates = GetComponents<PlayerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerStates.Length != 0 && GetComponent<SpriteRenderer>().enabled)
        {
            foreach (PlayerStates state in _playerStates)
            {
                state.LocalInput();
                state.ExecuteState();
                state.SetAnimation();
            }
        }
        
    }

    public void SpawnPlayer(Transform newPosition)
    {
        transform.position = newPosition.position;
    }

}
