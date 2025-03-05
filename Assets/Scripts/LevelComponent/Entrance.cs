using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    [SerializeField] private string levelName;
    private string holdButtonText = "Hold W or\nUPArrow";
    [SerializeField] private Image upArrow;
    [SerializeField] private TextMeshProUGUI levelNameText;
    [SerializeField] private GameObject entranceUI;
    [SerializeField] private string nextSceneName;
    private bool playerIsNear;
    private float holdTimeCount;
    private float maxHoldTimeCount = 1;
    private float textSwitchCount;
    private float maxTextSwitchCount= 2;

    // Start is called before the first frame update
    void Start()
    {
        entranceUI.SetActive(false);
        playerIsNear = false;
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsNear)
        {
            entranceUI.SetActive(true);
            //ゲージ溜め・メッセージ切り替え
            if(Input.GetKey(KeyCode.UpArrow)||Input.GetKey(KeyCode.W))
            {
                holdTimeCount += Time.deltaTime;
                if (holdTimeCount > maxHoldTimeCount)
                {
                    SoundBetweenScene.Instance.PlayEnterSE();
                    SceneManager.LoadScene(nextSceneName);
                }
                else
                    upArrow.fillAmount = holdTimeCount;
            }
            else
            {
                holdTimeCount -= Time.deltaTime * 1.5f;
                holdTimeCount = Mathf.Max(holdTimeCount, 0);
                upArrow.fillAmount = holdTimeCount;
            }
            SwitchingText();
        }
        else
        {
            //ゲージリセット・メッセージリセット
            Reset();
            entranceUI.SetActive(false);
        }
    }

    private void SwitchingText()
    {
        textSwitchCount += Time.deltaTime;
        if(textSwitchCount > maxTextSwitchCount)
        {
            textSwitchCount = 0;
            if (levelNameText.text == levelName)
                levelNameText.text = holdButtonText;
            else
                levelNameText.text = levelName;
        }
    }

    private void Reset()
    {
        holdTimeCount = 0;
        textSwitchCount = 0;
        levelNameText.text = levelName;
        upArrow.fillAmount = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerIsNear = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            playerIsNear = false;
    }
}
