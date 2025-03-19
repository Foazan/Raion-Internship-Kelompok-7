using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject interactMessage;
    [SerializeField] private GameObject menuText;
    [SerializeField] private GameObject menuBox;
    [SerializeField] private GameObject npcPortraitNormal; 
    [SerializeField] private GameObject npcPortraitAngry;  
    [SerializeField] private GameObject npcPortraitCenterNormal;
    [SerializeField] private GameObject npcPortraitCenterAngry;
    [SerializeField] private GameObject RestaurantBackground;
    [SerializeField] private GameObject LinneNormal;
    [SerializeField] private GameObject LinneAngry;
    public Image blackScreen;

    private Queue<string> textQueue = new Queue<string>();
    private Queue<string> nameQueue = new Queue<string>();
    private bool isTextDisplaying = false;
    public Canvas canvas;
    private Player player;

    private void Start()
    {
        HideMenuUI();
        interactMessage.SetActive(false);
        HideRestaurantBackground();
        npcPortraitNormal.SetActive(false);
        npcPortraitAngry.SetActive(false);
        npcPortraitCenterNormal.SetActive(false);
        RestaurantBackground.SetActive(false);
        ShowLinneNormal();
        speakerNameText.text = "";

        if (blackScreen == null)
            blackScreen = GetComponent<Image>();

        player = GameObject.FindWithTag("Player")?.GetComponent<Player>();
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
        ShowNpcPortraitCenterNormal();
        HideNpcPortrait();
    }

    public void ShowNpcPortraitCenterNormal()
    {
        npcPortraitCenterNormal.SetActive(true);
    }

    public void ShowNpcPortraitCenterAngry()
    {
        npcPortraitCenterAngry.SetActive(true);
    }

    public void HideNpcPortraitCenter()
    {
        npcPortraitCenterNormal.SetActive(false);
        npcPortraitCenterAngry.SetActive(false);
    }

    public IEnumerator ShowBlackScreen(float duration)
    {
        blackScreen.color = new Color(0, 0, 0, 1);
        yield return new WaitForSeconds(duration);
        blackScreen.color = new Color(0, 0, 0, 0);
    }

    public void ShowLinneNormal()
    {
        LinneNormal.SetActive(true);
        LinneAngry.SetActive(false);
    }

    public void ShowLinneAngry()
    {
        LinneAngry.SetActive(true);
        LinneNormal.SetActive(false);
    }

    public void SetNpcPortrait()
    {
        if (player != null)
        {
            float PlayerStress = player.getStress();
            if (PlayerStress < 50)
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

    private void HideNpcPortrait()
    {
        npcPortraitNormal.SetActive(false);
        npcPortraitAngry.SetActive(false);
    }
}
