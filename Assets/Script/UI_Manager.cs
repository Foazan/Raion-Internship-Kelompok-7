using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml.Serialization;
using System;

public class UI_Manager : MonoBehaviour
{
    private Player player;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI blackScreenText;
    public GameObject interactMessage;
    [SerializeField] private GameObject menuText;
    [SerializeField] private GameObject menuBox;
    [SerializeField] private GameObject npcPortraitNormal;
    [SerializeField] private GameObject npcPortraitAngry;
    [SerializeField] private GameObject npcPortraitCenterNormal;
    [SerializeField] private GameObject npcPortraitCenterAngry;
    [SerializeField] private GameObject RestaurantBackground;
    [SerializeField] private GameObject LinneNormal;
    [SerializeField] private GameObject LinneGloomy;
    public Image blackScreen;
    private Queue<string> textQueue = new Queue<string>();
    private Queue<string> nameQueue = new Queue<string>();
    private bool isTextDisplaying = false;
    public Canvas canvas;
    private bool start = true;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }

        menuText.SetActive(false);
        interactMessage.SetActive(false);
        HideRestaurantBackground();
        HideNpcPortrait();
        HideNpcPortraitCenter();
        RestaurantBackground.SetActive(false);
        speakerNameText.text = "";

        if (blackScreen == null)
            blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();

        UpdateMoneyText();
        ShowLinneNormal();
    }

    public void UpdateMoneyText()
    {
        moneyText.SetText(player.getMoney() + "$");
    }

    public void ShowText(string text, string speakerName = "")
    {
        textQueue.Enqueue(text);
        nameQueue.Enqueue(speakerName);
        if (!isTextDisplaying)
        {
            StartCoroutine(DisplayText());
        }
    }

    private IEnumerator DisplayText()
    {
        isTextDisplaying = true;

        while (textQueue.Count > 0)
        {
            dialogueText.text = "";
            string currentText = textQueue.Dequeue();
            string currentSpeaker = nameQueue.Dequeue();

            speakerNameText.text = string.IsNullOrEmpty(currentSpeaker) ? "" : currentSpeaker;

            SetNpcPortrait(); 

            foreach (char letter in currentText)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        isTextDisplaying = false;
        speakerNameText.text = "";

        HideNpcPortrait();
        HideNpcPortraitCenter();
    }

    public void ClearText()
    {
        textQueue.Clear();
        nameQueue.Clear();
        dialogueText.text = "";
        speakerNameText.text = "";
        isTextDisplaying = false;
    }

    public void ShowMenuUI()
    {
        menuText.SetActive(true);
        menuBox.SetActive(true);
    }

    public void HideMenuUI()
    {
        menuText.SetActive(false);
        menuBox.SetActive(false);
    }

    public void DisplayOrder()
    {
        StopAllCoroutines();
        isTextDisplaying = false;
    }

    public void HideText()
    {
        speakerNameText.text = "";
        dialogueText.text = "";
    }

    public void ShowInteractMessage()
    {
        interactMessage.SetActive(true);
    }

    public void HideInteractMessage()
    {
        interactMessage.SetActive(false);
    }

    public void HideRestaurantBackground()
    {
        HideNpcPortrait();
        HideNpcPortraitCenter();
        RestaurantBackground.SetActive(false);
    }

    public void ShowRestaurantBackground()
    {
        RestaurantBackground.SetActive(true);
        
    }

    public void ShowNpcPortraitCenterNormal()
    {
        npcPortraitCenterNormal.SetActive(true);
        npcPortraitCenterAngry.SetActive(false);
    }

    public void HideNpcPortraitCenter()
    {
        npcPortraitCenterNormal.SetActive(false);
        npcPortraitCenterAngry.SetActive(false);
    }

    public void HideNpcPortrait()
    {
        npcPortraitNormal.SetActive(false);
        npcPortraitAngry.SetActive(false);
    }

    public IEnumerator ShowBlackScreen(float duration, string text = "")
    {
        bool hasText = !string.IsNullOrEmpty(text);
        float halfDuration = duration / 2f;
        float elapsedTime = 0f;

        while (elapsedTime < halfDuration && !start)
        {
            float elapsedPercent = elapsedTime / halfDuration;
            blackScreen.color = Color.Lerp(Color.clear, Color.black, elapsedPercent);

            if (hasText)
            {
                blackScreenText.text = text;
                blackScreenText.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, elapsedPercent);
            }

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        blackScreen.color = Color.black;
        if (hasText) blackScreenText.color = Color.white;
        yield return new WaitForSeconds(duration/2);
        elapsedTime = 0f;
        halfDuration = duration / 2f;

        while (elapsedTime < halfDuration)
        {
            float elapsedPercent = elapsedTime / halfDuration;
            blackScreen.color = Color.Lerp(Color.black, Color.clear, elapsedPercent);
            blackScreenText.color = Color.Lerp(Color.white, new Color(1, 1, 1, 0), elapsedPercent);

            yield return null;
            elapsedTime += Time.deltaTime;
        }
        start = false;
        blackScreen.color = Color.clear;
        blackScreenText.text = "";
    }



    public void SetNpcPortrait()
    {
        if (player != null)
        {
            float playerStress = player.getStress();
            if (playerStress < 50)
            {
                npcPortraitNormal.SetActive(true);
                npcPortraitAngry.SetActive(false);
            }
            else
            {
                npcPortraitNormal.SetActive(false);
                npcPortraitAngry.SetActive(true);
            }
        }
    }

    public void ShowNpcPotraitCenterAngry()
    {
        npcPortraitCenterNormal.SetActive(false);
        npcPortraitCenterAngry.SetActive(true);
    }

    public void ShowLinneNormal()
    {
        LinneNormal.SetActive(true);
        LinneGloomy.SetActive(false);
    }

    public void ShowLinneGloomy()
    {
        LinneNormal.SetActive(false);
        LinneGloomy.SetActive(true);
    }
    public IEnumerator HideBlackScreen(float duration)
    {
        yield return duration;
    }
}
