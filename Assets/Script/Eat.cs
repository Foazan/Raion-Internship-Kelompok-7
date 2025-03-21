using UnityEngine;
using System.Collections;

public class Eat : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    [SerializeField] private float addedHunger;
    private bool isDayOne = true;

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
        if (!other.CompareTag("Player")) return;

        int currentDay = gameManager.GetCurrentDay();
        Debug.Log("Current Day: " + currentDay);
        Debug.Log("Current Time: " + gameManager.currentTime);

        if (isDayOne == true && gameManager.currentTime == "Pagi" && Input.GetKeyDown(KeyCode.E))
        {
            
           
                Debug.Log("Player mencoba makan di hari pertama. Hanya menampilkan teks.");
                uiManager.ShowText("I already ate", "Linne");
                isDayOne = false;
            
        }

        else if (isDayOne && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player memulai makan");
            StartEating();
        }
    }



    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractMessage();
        }
    }

    private void StartEating()
    {
        StartCoroutine(EatingRoutine());
    }

    private IEnumerator EatingRoutine()
    {
        
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "Nyam Nyam..."));
        gameManager.setNotSleeping();
        gameManager.AdvanceTime();
        player.addHunger(addedHunger);
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }
}
