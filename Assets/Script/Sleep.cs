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
        if (other.CompareTag("Player") && gameManager.currentTime == "Malam")
        {
            uiManager.ShowInteractMessage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.currentTime == "Malam")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartSleep();
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
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, $"Day {tomorrowDay} Begin...."));
        gameManager.setSleeping();
        gameManager.AdvanceTime();
        player.addSleep(addedSleep);
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
        
    }
}
