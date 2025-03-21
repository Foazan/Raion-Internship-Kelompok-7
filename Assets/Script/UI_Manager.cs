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
        ShowPrologue();
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


            foreach (char letter in currentText)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        isTextDisplaying = false;
        speakerNameText.text = "";
        dialogueText.text = "";

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

        while (elapsedTime < halfDuration)
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

    public void ShowRandomNPCDialogueRestaurant(bool isCorrectOrder)
    {
        string[] RestaaurantCorrectOrderResponses = {
        "Thank you.",
        "Yes, finally!!!",
        "I hope my friend likes this."
    };

        string[] RestaurantIncorrectOrderResponses = {
        "Ugh, can’t they afford better customer service.",
        "Wow this sure would look appetizing to my dog.",
        "UGH! You can’t even get that right???"
    };

        string selectedDialogue;

        if (isCorrectOrder)
        {
            selectedDialogue = RestaaurantCorrectOrderResponses[UnityEngine.Random.Range(0, RestaaurantCorrectOrderResponses.Length)];
            ShowNpcPortraitCenterNormal();
        }
        else
        {
            selectedDialogue = RestaurantIncorrectOrderResponses[UnityEngine.Random.Range(0, RestaurantIncorrectOrderResponses.Length)];
            ShowNpcPotraitCenterAngry();
        }

        ShowText(selectedDialogue, "Customer");
    }

    public void ShowPrologue()
    {
        string[] prologueTexts =
        {
        "It's been a while since that crash.",
        "...I have not been with my parents for a while now.",
        "I'm scared. I'm so scared. The house is awfully empty, and things get messy without someone cleaning them up.",
        "I feel awful. I’ve been eating frozen food, instant noodles, and take-out since they passed away. I can’t survive like this. I can’t keep living like this. I must do something. I have to.",
        "I have to try surviving by myself out there."
    };

        StartCoroutine(ShowBlackScreenWithText(3f, prologueTexts));
    }

    private IEnumerator ShowBlackScreenWithText(float fadeOutDuration, string[] texts)
    {
        blackScreen.color = Color.black;
        blackScreenText.color = Color.white;

        foreach (string text in texts)
        {
            blackScreenText.text = ""; 

            foreach (char letter in text)
            {
                blackScreenText.text += letter; 
                yield return new WaitForSeconds(0.02f); 
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); 
        }

        yield return new WaitForSeconds(1f);

        float elapsedTime = 0f;

        while (elapsedTime < fadeOutDuration)
        {
            float alpha = 1 - (elapsedTime / fadeOutDuration);
            blackScreen.color = new Color(0, 0, 0, alpha);
            blackScreenText.color = new Color(1, 1, 1, alpha);
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        blackScreen.color = Color.clear;
        blackScreenText.text = "";
        player.setPlayerCanWalk();
    }




}
