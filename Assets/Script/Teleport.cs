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
        bool isOutside = gameObject.name == "PintuRumahLuar";
        if (isOutside && currentDay == 1)
        {
            uiManager.ShowText("It’s still very early so there’s not many people.", "Linne");
            uiManager.ShowText("I should take a walk around the park. It would help me take my mind off things", "Linne");
            return;
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
