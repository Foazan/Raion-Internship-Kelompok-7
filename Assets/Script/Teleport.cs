using System;
using UnityEngine;
using System.Collections;
using static UnityEngine.Rendering.DebugUI;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform teleportTarget;
    [SerializeField] private bool isDoor = false;
    float duration = 2f;
    private bool isPlayerInRange = false;
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void OnTriggerEnter(Collider other)
    {
        int currentDay = gameManager.GetCurrentDay();
        String currentTime = gameManager.currentTime;
        bool isOutside = gameObject.name == "PintuRumahLuar";
        bool isInside = gameObject.name == "PintuRumahDalam";
        if (isOutside && currentDay == 1 && currentTime == "Pagi")
        {
            uiManager.ShowText("It’s still very early so there’s not many people.", "Linne");
            uiManager.ShowText("I should take a walk around the park. It would help me take my mind off things", "Linne");
            
        }
        if (isInside && currentDay == 2 && currentTime == "Malam")
        {
            uiManager.ShowText("Another day done. \nMeeting other people still makes me feel like I’m going to faint, \nbut at least I don’t feel the strong urge to run away.", "Linne");
        }
        if (isInside && currentDay == 4 && currentTime == "Malam")
        {
            uiManager.ShowText("I think I got a lot better at talking too. \nThe customers at work are nicer \nand my manager stopped scolding me so much.", "Linne");
        }
        if (isInside && currentDay == 6 && currentTime == "Malam")
        {
            uiManager.ShowText("Tomorrow’s going to be a new beginning…", "Linne");
        }
        if (isInside && currentDay == 7 && currentTime == "Malam")
        {
            uiManager.ShowText("I did it. I finally did it. \nI went outside regularly for a week. Ahaha! \nI can’t believe… I’m so proud of myself. \nFrom this day on, I promise I'll keep getting better at talking to others!", "Linne");
        }
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
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            bool isInHome = gameObject.name == "PintuKamarLuar";
            if (gameManager.currentTime == "Malam" && !isInHome)
            {
                uiManager.ShowText("I should stay at home.", "Linne");
                return;
            }
            else
            {
                TeleportPlayer();
            }
            
        }
    }

    private void TeleportPlayer()
    {
        if (player != null && teleportTarget != null)
        {
            StartCoroutine(uiManager.ShowBlackScreen(3f, "Moving...."));

            if (isDoor)
            {
                SoundManager.Instance.PlayDoorOpening();
            }

            StartCoroutine(waitTeleport(duration));
            
        }
    }

    private IEnumerator waitTeleport(float duration)
    {
        yield return new WaitForSeconds(duration);
        player.transform.position = teleportTarget.position;
        
    }
}
