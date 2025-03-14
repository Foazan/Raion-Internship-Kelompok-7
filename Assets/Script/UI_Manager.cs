using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class UI_Manager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // Komponen UI TextMeshPro untuk dialog
    [SerializeField] GameObject menuText;
    [SerializeField] GameObject menuBox;
    private Queue<string> textQueue = new Queue<string>();
    private bool isTextDisplaying = false;
    public Camera mainCamera;
    public Camera restaurantCamera;
    public Canvas canvas;


    private void Start()
    {
        menuBox.SetActive(false);
        menuText.SetActive(false);
    }

    private void Update()
    {
        
    }
    public void ShowText(string text)
    {
        textQueue.Enqueue(text);

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

            foreach (char letter in currentText)
            {
                dialogueText.text += letter;
                yield return new WaitForSeconds(0.02f); 
            }

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space)); 
        }

        isTextDisplaying = false;
    }

    public void ClearText()
    {
        textQueue.Clear();
        dialogueText.text = "";
        isTextDisplaying = false;
    }

    public void ShowMenuUI()
    {
        menuBox.SetActive(true);
        menuText.SetActive(true);
    }

    public void HideMenuUI()
    {
        menuBox.SetActive(false);
        menuText.SetActive(false);
    }

    public void ForceShowText(string text)
    {
        StopAllCoroutines(); // Hentikan animasi teks sebelumnya
        dialogueText.text = text; // Langsung ganti teks
        
            
    }

    public void SwitchToRestaurantView()
    {
        restaurantCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        canvas.worldCamera = restaurantCamera;
    }

    public void SwitchToMainView()
    {
        restaurantCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        canvas.worldCamera = mainCamera;
    }

    public void Displaytext()
    {
        StopAllCoroutines();
        isTextDisplaying = false;
    }
}
