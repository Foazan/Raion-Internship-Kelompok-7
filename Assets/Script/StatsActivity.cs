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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.currentTime != "Siang")
        {
            uiManager.ShowInteractMessage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        bool isRestaurant = gameObject.name == "Restaurant";
        bool isMinimarket = gameObject.name == "marketMinigame";
        if (other.CompareTag("Player") && gameManager.currentTime != "Siang" && !isMinimarket && !isRestaurant)
        {
            if (Input.GetKeyDown(KeyCode.E))
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

            return;
        }

        else if (BlackScreen)
        {
            StartCoroutine(uiManager.ShowBlackScreen(4f, blackScreenText));
        }
        StartCoroutine(WaitToBlack(3f));
        EndActivity();
    }

    protected override void EndActivity()
    {
        
        base.EndActivity();
    }

    private IEnumerator WaitToBlack(float a)
    {
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
        bool isPark = gameObject.name == "Gerbang_Taman";
        if (isPark && currentDay == 1 && gameManager.currentTime == "Siang")
        {
            uiManager.ShowText("Alright. I feel much better. Now I should try to get money.", "Linne");
        }
    }
   }
