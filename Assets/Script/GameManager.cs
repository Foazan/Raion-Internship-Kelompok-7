using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public string currentTime = "Pagi";
    private string[] timeBlocks = { "Pagi", "Siang", "Sore", "Malam" };
    private int currentBlockIndex = 0;
    private TimeFilterManager timeFilterManager;
    public Transform homePosition;
    public event Action<string> OnTimeBlockChanged;
    private UI_Manager uiManager;
    private Player player;
    public Camera mainCamera;
    public Camera restaurantCamera;
    public Camera minimarketCamera;
    public Canvas canvas;
    [SerializeField] private float addedHunger;
    [SerializeField] private float addedSleep;
    private float currentStress;
    private Boolean isSleeping = false;
    [SerializeField] private int currentDay = 1;
    private bool isGameOver;

    void Start()
    {
        timeFilterManager = GameObject.Find("TimeFilterManager").GetComponent<TimeFilterManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

        if (timeFilterManager == null)
        {
            Debug.LogError("TimeFilterManager tidak ditemukan di scene!");
        }
        else
        {
            Debug.Log("TimeFilterManager berhasil ditemukan!");
        }

        StartingToMainView();
        UpdateTimeBlock();
    }

    private void Update()
    {
        gameOverCheck();
    }

    public void AdvanceTime()
    {
        currentBlockIndex++;

        if (currentBlockIndex >= timeBlocks.Length)
        {
            currentBlockIndex = 0;
            currentDay++;
            Debug.Log("Hari Baru Dimulai!");
            
            if (isSleeping == true)
            {
                
            }
            else
            {
                player.addSleep(addedSleep);
            }
        }

        currentTime = timeBlocks[currentBlockIndex];
        UpdateTimeBlock();

        if (currentTime == "Malam")
        {
            StartCoroutine(MovePlayerHome());
        }

        
    }

    public int GetCurrentDay()
    {
        return currentDay;
    }

    public Boolean getSleeping()
    {
        return isSleeping;
    }

    public void setSleeping()
    {
        isSleeping = true;
    }

    public void setNotSleeping()
    {
        isSleeping = false;
    }
    void UpdateTimeBlock()
    {
        Debug.Log("Waktu sekarang: " + currentTime);
        OnTimeBlockChanged?.Invoke(currentTime);

        if (timeFilterManager != null)
        {
            if (currentTime == "Pagi")
                timeFilterManager.SetMorningFilter();
            else if (currentTime == "Siang")
                timeFilterManager.SetNoonFilter();
            else if (currentTime == "Sore")
                timeFilterManager.SetEveningFilter();
            else if (currentTime == "Malam")
                timeFilterManager.SetNightFilter();
        }
    }

    IEnumerator MovePlayerHome()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && homePosition != null)
        {
            yield return new WaitForSeconds(5f);
            StartCoroutine(uiManager.ShowBlackScreen(3f, "Night Has Come, Better Go Home....")); 
            StartCoroutine(waitTeleport(2f));
        }
        else
        {
            Debug.LogWarning("Player atau Home Position tidak ditemukan!");
        }
    }


    public void SwitchToRestaurantView()
    {
        TransitionToRestaurant();
        Debug.Log("Beralih ke tampilan restoran.");
    }

    public void SwitchtoMinimarketView()
    {
        TransitionToMinikmarket();
    }

    public void SwitchToMainView()
    {
        TransitionToMainView();
        Debug.Log("Beralih ke tampilan utama.");
    }

    public void StartingToMainView()
    {
        StartToMainView();
        Debug.Log("Beralih ke tampilan utama.");
    }

    public void TransitionToRestaurant()
    {
        StartCoroutine(uiManager.ShowBlackScreen(2f, "Enter the Restaurant...."));
        StartCoroutine(waitSwitchCamera(restaurantCamera, mainCamera, 2f));
    }

    private void TransitionToMinikmarket()
    {
        StartCoroutine(uiManager.ShowBlackScreen(2f, "Enter the Minimarket...."));
        StartCoroutine(waitSwitchCamera(minimarketCamera, mainCamera, 2f));
    }

    private void TransitionToMainView()
    {
        StartCoroutine(uiManager.ShowBlackScreen(2f, "Leaving...."));
        StartCoroutine(waitSwitchCamera(mainCamera, restaurantCamera, 2f)); 
    }

    private void StartToMainView()
    {
        //StartCoroutine(uiManager.ShowBlackScreen(3f));
        StartCoroutine(waitSwitchCamera(mainCamera, restaurantCamera, 2f));
    }
    public void SwitchToMinimarketView()
    {
        TransitionToRestaurant();
    }

    public bool gameOverCheck()
    {
        currentStress = player.getStress();
        if (currentStress >= 100)
        {
            isGameOver = true;
            StartCoroutine(gameOver());
        }
        return isGameOver;
    }
    private IEnumerator gameOver()
    {
        yield return uiManager.ShowBlackScreen(2f, "Linne pulang dan KHS");
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private IEnumerator waitSwitchCamera(Camera a,Camera b, float duration)
    {
        yield return new WaitForSeconds(duration);
        a.enabled = true;
        b.enabled = false;
        canvas.worldCamera = a;
    }

    private IEnumerator waitTeleport(float duration)
    {
        yield return new WaitForSeconds(duration);
        player.transform.position = homePosition.position;
        
    }
}
