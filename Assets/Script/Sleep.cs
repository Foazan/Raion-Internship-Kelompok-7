using UnityEngine;
using System.Collections;

public class Sleep : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
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
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f));
        gameManager.AdvanceTime();
    }
}
