using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceTile : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private Vector2 direction;


    private PlayerController _playerController;


    private void Update()
    {
        if (_playerController == null)
        {
            return;
        }

        _playerController.SetHorizontalForce(direction.x*10);
        _playerController.SetVerticalForce(direction.y * 10);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            _playerController = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _playerController = null;
    }
}
