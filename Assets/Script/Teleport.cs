using System;
using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform teleportTarget;   
    private bool isPlayerInRange = false;
    private GameManager gameManager;
    private UI_Manager uiManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    private IEnumerator TeleportPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && teleportTarget != null)
        {
            player.transform.position = teleportTarget.position;
            yield return StartCoroutine(uiManager.ShowBlackScreen(1f)); 
        }
    }
}
