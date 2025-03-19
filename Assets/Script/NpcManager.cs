using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    private UI_Manager uiManager;
    private bool isTalking = false;
    private Player player;
    private float PlayerStress;


    private void Start()
    {
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowInteractMessage();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
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
        if (player != null)
        {
            float PlayerStress = player.getStress();

            if (PlayerStress > 20)
            {
                uiManager.ShowLinneGloomy();
            }
            else
            {
                uiManager.ShowLinneNormal();
            }
        }
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
        uiManager.ShowLinneNormal();
    }
}
