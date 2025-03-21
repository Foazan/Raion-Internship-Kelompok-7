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
    protected override void Start()
    {
        base.Start();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    protected override void Update()
    {
        base.Update();
    }
    
    protected override void StartActivity()
    {
        base.StartActivity();
        if (BlackScreen)
        {
            StartCoroutine(uiManager.ShowBlackScreen(4f,blackScreenText));
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
        player.addStress(Stress);
        player.addHunger(Hunger);
        player.addSleep(Sleep);
        player.addMoney(MoneyCost);
    }
   }
