using UnityEngine;
using System.Collections;
using System;

public class Sleep : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    
    [SerializeField] private float addedSleep;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowInteractMessage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int currentDay = gameManager.GetCurrentDay();
        if (other.CompareTag("Player") && gameManager.currentTime == "Malam")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSleep();
            }
        }

        else if (other.CompareTag("Player") && currentDay == 1 && gameManager.currentTime != "Malam") 
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                uiManager.ShowText("...No, I can't keep living like this.", "Linne");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractMessage();
        }
    }

    private void StartSleep()
    {
        StartCoroutine(SleepRoutine());
    }


    private IEnumerator SleepRoutine()
    {
        int tomorrowDay = gameManager.GetCurrentDay() + 1;
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "I want to buy softer pillows…"));
        gameManager.setSleeping();
        gameManager.AdvanceTime();
        player.addSleep(addedSleep);
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
        
    }
}
