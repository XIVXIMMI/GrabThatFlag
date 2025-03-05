using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConditions : MonoBehaviour
{
    public bool IsCollidingBelow { get; set; }
    public bool IsCollidingAbove { get; set; }
    public bool IsCollidingRight { get; set; }
    public bool IsCollidingLeft { get; set; }
    
    public bool IsFalling { get; set; }
    public bool IsWallClinging { get; set; }
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }

    public float fallMultiplier;
    public float gravity;
    public float currentFallMultiplier;
    public float currentGravity;

    public void Reset()
    {
        IsCollidingAbove = false;
        IsCollidingBelow = false;
        IsCollidingLeft = false;
        IsCollidingRight = false;

        IsFalling = false;

        currentFallMultiplier = fallMultiplier;
        currentGravity = gravity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
