using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHair : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private Vector3 mouseWorldPos;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0;
        this.transform.position = mouseWorldPos;
        //this.transform.position = player.transform.position+(mouseWorldPos - player.transform.position).normalized*1.5f;
    }
}
