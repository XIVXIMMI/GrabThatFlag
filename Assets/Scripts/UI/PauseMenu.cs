using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenuUI;
    [SerializeField] AudioClip pauseSE;
    private AudioSource audioSource;
    private MenuController menuController;
    public bool preventOpenPause;

    // Start is called before the first frame update
    void Start()
    {
        preventOpenPause = false;
        pauseMenuUI.SetActive(false);
        menuController = GetComponent<MenuController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!preventOpenPause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Pause();
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        audioSource.PlayOneShot(pauseSE);
        if (pauseMenuUI.activeSelf)
        {
            Time.timeScale = 0;
            menuController.SetFirstButton();
        }
        else
            Time.timeScale = 1;
    }
}
