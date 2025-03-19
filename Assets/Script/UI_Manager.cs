using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Xml.Serialization;

public class UI_Manager : MonoBehaviour
{
    private Player player;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI moneyText;
    public GameObject interactMessage;
    [SerializeField] private GameObject menuText;
    [SerializeField] private GameObject npcPortrait;
    [SerializeField] private GameObject npcPortraitCenter;
    [SerializeField] private GameObject RestaurantBackground;
    public Image blackScreen;
    private Queue<string> textQueue = new Queue<string>();
    private Queue<string> nameQueue = new Queue<string>();
    private bool isTextDisplaying = false;
    public Canvas canvas;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        menuText.SetActive(false);
        interactMessage.SetActive(false);
        HideRestaurantBackground();
        npcPortrait.SetActive(false);
        npcPortraitCenter.SetActive(false);
        RestaurantBackground.SetActive(false);
        speakerNameText.text = "";
        if (blackScreen == null)
            blackScreen = GameObject.Find("BlackScreen").GetComponent<Image>();
        updateMoneyText();
    }

    private void updateMoneyText()
    {
        moneyText.SetText(player.getMoney()+"$");
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

            ShowNpcPotrait();

            foreach (char letter in currentText)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        isTextDisplaying = false;
        speakerNameText.text = "";

        HideNpcPotrait();
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
    }

    public void HideMenuUI()
    {
        menuText.SetActive(false);
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

    public void ShowNpcPotrait()
    {
        if (!npcPortraitCenter.activeSelf)
        {
            npcPortrait.SetActive(true);
        }
    }

    public void HideNpcPotrait()
    {
        npcPortrait.SetActive(false); 
    }

    public void HideRestaurantBackground()
    {
        npcPortrait.SetActive(false);
        HideNpcPortraitCenter();
        RestaurantBackground.SetActive(false);
    }

    public void ShowRestaurantBackground()
    {
        RestaurantBackground.SetActive(true);
        ShowNpcPortraitCenter();
        npcPortrait.SetActive(false);
    }

    public void ShowNpcPortraitCenter()
    {
        npcPortraitCenter.SetActive(true);
    }

    public void HideNpcPortraitCenter()
    {
        npcPortraitCenter.SetActive(false);
    }

    public IEnumerator ShowBlackScreen(float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float elapsedPercent = elapsedTime / (duration/2);
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), elapsedPercent);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        blackScreen.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(0.5f);
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float elapsedPercent = elapsedTime / (duration/2);
            blackScreen.color = Color.Lerp(new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), elapsedPercent);

            yield return null;
            elapsedTime += Time.deltaTime;
        }

        blackScreen.color = new Color(0, 0, 0, 0);
    }

}
