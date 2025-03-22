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
        int currentDay = gameManager.GetCurrentDay();
        String currentTime = gameManager.currentTime;
        if (other.CompareTag("Player"))
        {
            uiManager.ShowInteractMessage();

            if (currentDay == 2 && currentTime == "Pagi")
            {
                uiManager.ShowText("Ugh, yesterday was terrible. Do I really have to go out? \nCan’t I just go back to staying home?", "Linne");
                uiManager.ShowText("...*sigh* I won’t give up so easily. \nMy parents aren’t here to take care of me anymore. \nEven if they were still here, \nI don’t want them to be forced to take care of me forever. \nI need to get out of my head and live in the real world, \nand in the real world I need to meet people regularly", "Linne");
            }
            if (currentDay == 4 && currentTime == "Pagi")
            {
                uiManager.ShowText("This isn’t so bad. \nThe routine of going out makes me take care of myself a bit more too. \nI think I can get used to this.", "Linne");
            }
            if (currentDay == 6 && currentTime == "Pagi")
            {
                uiManager.ShowText("Tomorrow will mark the 7th day of going out regularly. \nI can’t believe I even reached this point. \nMy reactions to talking to people were so extreme \nI thought I could never get over it. \nI hope I could keep getting better", "Linne");
            }
            if (currentDay == 7 && currentTime == "Pagi")
            {
                uiManager.ShowText("Today is the 7th day. I just need to get through this day \nand I would have gone outside regularly for a week. \nHm… maybe I could even treat myself something nice to celebrate?", "Linne");
            }
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
