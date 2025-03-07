using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public string currentTime = "Pagi"; // Mulai dari pagi
    private string[] timeBlocks = { "Pagi", "Siang", "Sore", "Malam" }; 
    private int currentBlockIndex = 0;

    public Transform homePosition; 
    public event Action<string> OnTimeBlockChanged; 

    void Start()
    {
        UpdateTimeBlock();
    }

    public void AdvanceTime()
    {
        currentBlockIndex++;

        if (currentBlockIndex >= timeBlocks.Length)
        {
            // Reset ke pagi hari berikutnya
            currentBlockIndex = 0;
            Debug.Log("Hari Baru Dimulai!");
        }

        currentTime = timeBlocks[currentBlockIndex];
        UpdateTimeBlock();

        if (currentTime == "Malam")
        {
            // Saat malam, otomatis pulang ke rumah
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                player.transform.position = homePosition.position;
            }
        }
    }

    void UpdateTimeBlock()
    {
        Debug.Log("Waktu sekarang: " + currentTime);
        OnTimeBlockChanged?.Invoke(currentTime); // Beri tahu sistem lain kalau waktu berubah
    }
}
