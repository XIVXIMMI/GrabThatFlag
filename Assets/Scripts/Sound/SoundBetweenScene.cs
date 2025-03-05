using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundBetweenScene : MonoBehaviour
{
    // Start is called before the first frame update
    private AudioSource audioSource;
    [SerializeField] private AudioClip clickSE;
    [SerializeField] private AudioClip enterSE;

    public static SoundBetweenScene Instance
    {
        get; private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
    }

    public void PlayClickSE()
    {
        audioSource.PlayOneShot(clickSE);
    }

    public void PlayEnterSE()
    {
        audioSource.PlayOneShot(enterSE);
    }
}