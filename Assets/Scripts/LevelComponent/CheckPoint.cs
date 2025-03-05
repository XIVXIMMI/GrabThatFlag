using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Animator animator;
    LevelManager levelManager;
    [SerializeField] AudioClip checkPointSE;
    AudioSource audioSource;
    bool playOnce;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        audioSource = GetComponent<AudioSource>();
        playOnce = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            animator.SetBool("Activate", true);
            if (levelManager != null)
                levelManager.SetSpawnPoint(this.transform);
            if(!playOnce)
            {
                playOnce = true;
                audioSource.PlayOneShot(checkPointSE);
            }
        }
    }
}
