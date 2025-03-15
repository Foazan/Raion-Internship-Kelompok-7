using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public GameObject interactMessage;
    [SerializeField] GameObject menuText;
    private Queue<string> textQueue = new Queue<string>();
    private Queue<string> nameQueue = new Queue<string>(); 
    private bool isTextDisplaying = false;
    public Canvas canvas;

    private void Start()
    {
        
        menuText.SetActive(false);
        interactMessage.SetActive(false);
        speakerNameText.text = ""; 
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

}
