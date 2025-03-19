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

        SwitchToMainView();
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
            MovePlayerHome();
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

    void MovePlayerHome()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && homePosition != null)
        {
            player.transform.position = homePosition.position;
            Debug.Log("Player otomatis pulang ke rumah.");
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

    private IEnumerator TransitionToRestaurant()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(1f)); 
        mainCamera.enabled = false;
        restaurantCamera.enabled = true;
        canvas.worldCamera = restaurantCamera;
    }

    private IEnumerator TransitionToMainView()
    {
        yield return StartCoroutine(uiManager.ShowBlackScreen(1f)); 
        mainCamera.enabled = true;
        restaurantCamera.enabled = false;
        canvas.worldCamera = mainCamera;
    }
    public void SwitchToMinimarketView()
    {
        minimarketCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
    }
}
