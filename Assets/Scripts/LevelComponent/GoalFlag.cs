using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFlag : MonoBehaviour
{
    [SerializeField] private GameObject clearUI;
    [SerializeField] private AudioClip goalSE;
    [SerializeField] private PauseMenu pauseMenu;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        clearUI.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            pauseMenu.preventOpenPause = true;
            Time.timeScale = 0;
            clearUI.SetActive(true);
            audioSource.PlayOneShot(goalSE);
        }
    }
}
