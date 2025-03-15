using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    private bool isPlayerInRange;
    private UI_Manager uiManager;
    private bool isTalking = false;


    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            uiManager.ShowInteractMessage();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            uiManager.HideInteractMessage();
        }
    }

    void Update()
    {
       
    }

    public void Interact()
    {
        if (isTalking) return;
        uiManager.HideInteractMessage();
        isTalking = true;
        StartDialogue();
    }

    private void StartDialogue()
    {
        uiManager.ShowText("Who are you?", "Old Man");
        

        uiManager.ShowText("Keep your own business", "Old Man");
        

        uiManager.ShowText("Oh, I am sorry", "Linne");

        StartCoroutine(EndDialogue());
    }

    IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        uiManager.HideText();
        isTalking = false;
    }
}
