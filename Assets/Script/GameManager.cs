using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
    public string currentTime = "Pagi";
    private string[] timeBlocks = { "Pagi", "Siang", "Malam" };
    private int currentBlockIndex = 0;
    private TimeFilterManager timeFilterManager;
    public Transform homePosition;
    public event Action<string> OnTimeBlockChanged;
    private UI_Manager uiManager;

    public Camera mainCamera;
    public Camera restaurantCamera;
    public Camera minimarketCamera;
    public Canvas canvas;

    void Start()
    {
        timeFilterManager = GameObject.Find("TimeFilterManager").GetComponent<TimeFilterManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

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

    public void AdvanceTime()
    {
        currentBlockIndex++;

        if (currentBlockIndex >= timeBlocks.Length)
        {
            currentBlockIndex = 0;
            Debug.Log("Hari Baru Dimulai!");
        }

        currentTime = timeBlocks[currentBlockIndex];
        UpdateTimeBlock();

        if (currentTime == "Malam")
        {
            StartCoroutine(MovePlayerHome());
        }
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
            else if (currentTime == "Malam")
                timeFilterManager.SetNightFilter();
        }
    }

    IEnumerator MovePlayerHome()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && homePosition != null)
        {
            yield return StartCoroutine(uiManager.ShowBlackScreen(3f, "Night Has Come, Better Go Home....")); 
            player.transform.position = homePosition.position;
            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(uiManager.HideBlackScreen(3f)); 
        }
        else
        {
            Debug.LogWarning("Player atau Home Position tidak ditemukan!");
        }
    }


    public void SwitchToRestaurantView()
    {
        StartCoroutine(TransitionToRestaurant());
        Debug.Log("Beralih ke tampilan restoran.");
    }

    public void SwitchToMainView()
    {
        StartCoroutine(TransitionToMainView());
        Debug.Log("Beralih ke tampilan utama.");
    }

    public void StartingToMainView()
    {
        StartCoroutine(StartToMainView());
        Debug.Log("Beralih ke tampilan utama.");
    }

    private IEnumerator TransitionToRestaurant()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "Start Working....")); 
        mainCamera.enabled = false;
        restaurantCamera.enabled = true;
        canvas.worldCamera = restaurantCamera;
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }

    private IEnumerator TransitionToMainView()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(2f, "Leaving....")); 
        mainCamera.enabled = true;
        restaurantCamera.enabled = false;
        canvas.worldCamera = mainCamera;
        yield return StartCoroutine(uiManager.HideBlackScreen(2f));
    }

    private IEnumerator StartToMainView()
    {
        StartCoroutine(uiManager.ShowBlackScreen(0f, "Loading Game...."));
        mainCamera.enabled = true;
        restaurantCamera.enabled = false;
        canvas.worldCamera = mainCamera;
        yield return new WaitForSeconds(2f);
        StartCoroutine(uiManager.HideBlackScreen(1f));
    }
    public void SwitchToMinimarketView()
    {
        minimarketCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
}
