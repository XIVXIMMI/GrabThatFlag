using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuSE : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip mainSE;
    [SerializeField] private AudioClip subSE;
    [SerializeField] private AudioClip fallSE;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayMainSE()
    {
        audioSource.PlayOneShot(mainSE);
    }

    public void PlaySubSE()
    {
        audioSource.PlayOneShot(subSE);
    }

    public void FallSE()
    {
        audioSource.PlayOneShot(fallSE);
    }
}
