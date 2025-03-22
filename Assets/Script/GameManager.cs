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
    private bool isGameWinning;

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
        if (currentTime == "Malam") 
        {
            currentBlockIndex = 0; 
            currentDay++; 
            Debug.Log("Hari Baru Dimulai!");
            isSleeping = false; 
        }
        else
        {
            currentBlockIndex++; 
        }

        currentTime = timeBlocks[currentBlockIndex]; 
        UpdateTimeBlock();

        if (currentTime == "Malam")
        {
            StartCoroutine(MovePlayerHome()); 
        }
        gameWinningCheck();
    }


    public int GetCurrentDay()
    {
        return currentDay;
    }

    public String GetCurrentTime()
    {
        return currentTime;
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

    public void SwitchFromMinimarket()
    {
        StartCoroutine(uiManager.ShowBlackScreen(2f, "Leaving...."));
        StartCoroutine(waitSwitchCamera(mainCamera, minimarketCamera, 2f));
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

    public bool gameWinningCheck()
    {
        currentStress = player.getStress();
        if (currentStress < 100 && currentDay >= 7)
        {
            StartCoroutine(WinningScreen());
        }
        return isGameWinning;
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

    private void CheckWinningCondition()
    {
        if (currentDay > 9 && player.getStress() < 100)
        {
            StartCoroutine(WinningScreen());
        }
    }

    private IEnumerator WinningScreen()
    {
        yield return uiManager.ShowBlackScreen(5f,
            "It’s been a week since I started going out. Meeting people is still terrifying, but I handle interacting with people better now. I hope one day I could befriend someone.");

        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

}
