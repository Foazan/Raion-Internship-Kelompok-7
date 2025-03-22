using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class StatsActivity : Activity
{
    private Player player;

    [SerializeField] private int MoneyCost;
    [SerializeField] private float Stress, Hunger, Sleep;
    [SerializeField] private Boolean BlackScreen;
    [SerializeField] private String blackScreenText;
    [SerializeField] private TextMeshProUGUI screenText;
    [SerializeField] private bool isAdvanceTime = false;
    




    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").GetComponent<Player>();
        
    }
    protected override void Update()
    {
        base.Update();
        

    }

    public void OnTriggerEnter(Collider other)
    {
        int currentDay = gameManager.GetCurrentDay();
        String currentTime = gameManager.GetCurrentTime();
        bool isRestaurant = gameObject.name == "Restaurant";
        bool isMinimarket = gameObject.name == "marketMinigame";
        bool isPark = gameObject.name == "Gerbang_Taman";
        if (other.CompareTag("Player"))
        {
            if (gameManager.currentTime != "Siang")
            {
                
                if (currentDay == 1 && (isRestaurant || isMinimarket))
                {
                    uiManager.ShowTutorial("Linne can work to earn money, money can be spent on activities outside of her home.");

                }
                else if (currentDay == 1 && isPark)
                {
                    uiManager.ShowTutorial("Doing certain activities will affect Linne's stress, hunger, and sleep meter. Try to keep them balanced.");

                }
                uiManager.ShowInteractMessage();
            }
        }
    }


    private void OnTriggerStay(Collider other)
    {
        bool isRestaurant = gameObject.name == "Restaurant";
        bool isMinimarket = gameObject.name == "marketMinigame";
        if (other.CompareTag("Player") && !isMinimarket && !isRestaurant)
        {
            if (Input.GetKeyDown(KeyCode.E) && gameManager.currentTime != "Siang")
            {
                StartActivity();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.currentTime != "Siang")
        {
            uiManager.HideInteractMessage();
        }
    }

    public override void StartActivity()
    {
        base.StartActivity();
        currentDay = gameManager.GetCurrentDay();
        bool isPark = gameObject.name == "Gerbang_Taman";
        bool isEating = gameObject.name == "Kulkas";
        bool isGaming = gameObject.name == "Game";

        if (currentDay == 1)
        {
            if (isEating)
            {
                uiManager.ShowText("I already ate", "Linne");
                return;
            }

            if (isGaming)
            {
                uiManager.ShowText("No, no. Things must change", "Linne");
                return;
            }
            if (gameManager.currentTime == "Pagi" && !isPark)
            {
                uiManager.ShowText("I should take a walk around the park.", "Linne");
                return;
            }
        }

        StartCoroutine(HandleActivityStart());
    }


    protected override void EndActivity()
    {
        
        base.EndActivity();
    }

    private IEnumerator WaitToBlack(float a)
    {
        bool isPark = gameObject.name == "Gerbang_Taman";
        
        yield return new WaitForSeconds(a);

        gameManager.setNotSleeping();
        player.addStress(Stress);
        player.addHunger(Hunger);
        player.addSleep(Sleep);
        player.addMoney(MoneyCost);

        if (isAdvanceTime == true)
        {
            gameManager.AdvanceTime();
        }
        if (isPark && currentDay == 1 && gameManager.currentTime == "Siang")
        {
            uiManager.ShowText("Alright. I feel much better. Now I should try to get money.", "Linne");
        }
    }

    private IEnumerator HandleActivityStart()
    {
        if (BlackScreen)
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(4f, blackScreenText)); 
    }

        yield return StartCoroutine(WaitToBlack(3f)); 
        EndActivity();
    }
}
