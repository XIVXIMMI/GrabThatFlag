using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(AudioSource))]
public class ButtonColor : MonoBehaviour
{
    private Image button;
    public bool isSelected = false;
    private AudioSource audioSource;
    private Animator animator;
    [SerializeField] AudioClip selectSE;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        button = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            animator.SetBool("Selected", true);
            if (!isSelected)
            {
                isSelected = true;
                audioSource.PlayOneShot(selectSE);
            }
        }
        else if(isSelected)
        {
            button.color = new Color(255, 255, 255, 1);
            animator.SetBool("Selected", false);
            isSelected = false;
        }
    }


    public void PlayClickSE()
    {
        SoundBetweenScene.Instance.PlayClickSE();
    }
}
