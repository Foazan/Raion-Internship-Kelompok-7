using UnityEngine;
using System.Collections;

public class Gaming : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    [SerializeField] float addedStress;

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
                StartGaming();
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

    private void StartGaming()
    {
        StartCoroutine(GamingRoutine());
    }

    private IEnumerator GamingRoutine()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "Playing Games Until Morning...."));
        gameManager.StayUpLate();
        gameManager.AdvanceTime();
        player.addStress(-addedStress);
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }
}
