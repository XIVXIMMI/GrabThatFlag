using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public enum MoveDirections
    {
        LEFT, RIGHT
    }

    [Header("Settings")]
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float minDistanceToPoint = 0.1f;

    public float MoveSpeed => moveSpeed;

    public MoveDirections Direction { get; set; }

    public List<Vector3> points = new List<Vector3>();  // Get the Point list

    private bool _playing;
    private bool _moved;
    private int _currentPoint = 0;
    private Vector3 _currentPosition;
    private Vector3 _previousPosition;

    private void Start()
    {
        _playing = true;

        _previousPosition = transform.position;
        _currentPosition = transform.position;
        transform.position = _currentPosition + points[0];
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        if (points.Count > 1)
        {// Set first position
            if (!_moved)
            {
                transform.position = _currentPosition + points[0];
                _currentPoint++;
                _moved = true;
            }

            // Move to next point
            transform.position = Vector3.MoveTowards(transform.position, _currentPosition + points[_currentPoint],
                                 Time.deltaTime * moveSpeed);

            // Evaluate move to next point
            float distanceToNextPoint = Vector3.Distance(_currentPosition + points[_currentPoint], transform.position);
            if (distanceToNextPoint < minDistanceToPoint)
            {
                _previousPosition = transform.position;
                _currentPoint++;
            }

            // Define move direction
            if (_previousPosition != Vector3.zero)
            {
                if (transform.position.x > _previousPosition.x)
                {
                    Direction = MoveDirections.RIGHT;
                }
                else if (transform.position.x < _previousPosition.x)
                {
                    Direction = MoveDirections.LEFT;
                }
            }

            // If we are on the last point, reset our position to the first one
            if (_currentPoint == points.Count)
            {
                _currentPoint = 0;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (transform.hasChanged && !_playing)
        {
            _currentPosition = transform.position;
        }

        if (points != null)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (i < points.Count)
                {
                    // Draw points
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireSphere(_currentPosition + points[i], 0.4f);

                    // Draw lines
                    Gizmos.color = Color.black;
                    if (i < points.Count - 1)
                    {
                        Gizmos.DrawLine(_currentPosition + points[i], _currentPosition + points[i + 1]);
                    }

                    // Draw line from last point to first point
                    if (i == points.Count - 1)
                    {
                        Gizmos.DrawLine(_currentPosition + points[i], _currentPosition + points[0]);
                    }
                }
            }
        }
    }
}