using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedButton, optionButton, optionCloseButton;
    GameObject lastSelection = null;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Update()
    {
        if(lastSelection != null)
        {
            if(EventSystem.current.currentSelectedGameObject == null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(lastSelection);
            }
        }
        if (EventSystem.current.currentSelectedGameObject != null)
            lastSelection = EventSystem.current.currentSelectedGameObject;
        if (lastSelection == optionCloseButton)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(lastSelection);
        }
    }

    public void SetFirstButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }

    public void OnOptionButtonClick()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionCloseButton);
    }

    public void OnOptionClose()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(optionButton);
    }
}
