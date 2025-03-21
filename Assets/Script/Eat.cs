using UnityEngine;
using System.Collections;

public class Eat : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    [SerializeField] private float addedHunger;

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
                StartEating();
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

    private void StartEating()
    {
        StartCoroutine(EatingRoutine());
    }

    private IEnumerator EatingRoutine()
    {
        int tomorrowDay = gameManager.GetCurrentDay() + 1;
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "Eating Until" + $"Day {tomorrowDay} Begin...."));
        gameManager.StayUpLate();
        gameManager.AdvanceTime();
        player.addHunger(addedHunger);
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }
}
