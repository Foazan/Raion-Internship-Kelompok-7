using UnityEngine;
using System.Collections;
public class Park : MonoBehaviour
{
    private GameManager gameManager;
    private UI_Manager uiManager;
    private Player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.currentTime != "Malam")
        {
            uiManager.ShowInteractMessage();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.currentTime != "Malam")
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
        player.addStress(-5f);
        gameManager.AdvanceTime();
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }
}
