using UnityEngine;
using System.Collections;
public class Park : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    [SerializeField] private float addedStress;
    [SerializeField] private float Cost;
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
        if (other.CompareTag("Player") && (gameManager.currentTime == "Sore" || gameManager.currentTime == "Pagi"))
        {
            uiManager.ShowInteractMessage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && (gameManager.currentTime == "Sore" || gameManager.currentTime == "Pagi"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartPark();
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

    private void StartPark()
    {
        StartCoroutine(ParkRoutine());
    }

    private IEnumerator ParkRoutine()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "The Park is Beutiful Today...."));
        player.addStress(-addedStress);
        player.addMoney(-Cost);
        gameManager.AdvanceTime();
        uiManager.UpdateMoneyText();
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }
}
