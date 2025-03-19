﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RestaurantActivity : Activity
{
    private Dictionary<string, int> playerOrder = new Dictionary<string, int>();
    private Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private bool isOrdering = false;
    private System.Random random = new System.Random();

    private string[] menuItems =
    {
        "Burger", "Pizza", "Soda", "Fried Chicken", "Noodles",
        "Coffee", "Ice Cream", "Salad", "Steak"
    };

    private Player player;

    protected override void Start()
    {
        base.Start();
        activityName = "Melayani Pelanggan";
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    protected override void Update()
    {
        base.Update();
        if (!isOrdering) return;

        for (int i = 0; i < menuItems.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                AddItem(menuItems[i]);
                return;
            }
        }
    }

    protected override void StartActivity()
    {
        gameManager.SwitchToRestaurantView();
        uiManager.HideInteractMessage();
        if (isOrdering) return;
        isOrdering = true;
        playerOrder.Clear();
        customerOrder.Clear();
        uiManager.ShowRestaurantBackground();
        GenerateRandomOrder();

        uiManager.ShowText($"Memulai {activityName}...", "Kasir");
        uiManager.ShowText($"Saya akan memesan {GetTotalOrderCount()} item.", "Pelanggan");

        ShowOrderSummary();
        ShowMenu();
    }

    private void GenerateRandomOrder()
    {
        int orderCount = random.Next(2, 5);
        customerOrder.Clear();

        for (int i = 0; i < orderCount; i++)
        {
            string randomItem = menuItems[random.Next(menuItems.Length)];
            if (!customerOrder.ContainsKey(randomItem))
                customerOrder[randomItem] = 0;

            customerOrder[randomItem]++;
        }
    }

    private int GetTotalOrderCount()
    {
        int total = 0;
        foreach (var item in customerOrder)
        {
            total += item.Value;
        }
        return total;
    }

    private void ShowOrderSummary()
    {
        uiManager.ShowText($"Saya Mau Pesan:", "Pelanggan");
        foreach (var item in customerOrder)
        {
            uiManager.ShowText($"- {item.Key} x{item.Value}", "Pelanggan");
        }
    }

    private void ShowMenu()
    {
        uiManager.ShowMenuUI();
        uiManager.ShowText("Silakan pilih menu.", "Kasir");
    }

    private void AddItem(string item)
    {
        if (!playerOrder.ContainsKey(item))
            playerOrder[item] = 0;

        playerOrder[item]++;
        uiManager.DisplayOrder();

        ShowAddedItem(item);

        if (IsOrderComplete())
        {
            uiManager.ShowText("Anything else? Tekan Spasi jika pesanan selesai.", "Kasir");
            ConfirmOrder();
        }
    }

    private bool IsOrderComplete()
    {
        int totalCustomerOrder = customerOrder.Values.Sum();
        int totalPlayerOrder = playerOrder.Values.Sum();

        return totalPlayerOrder == totalCustomerOrder;
    }

    private void ConfirmOrder()
    {
        uiManager.ShowText("Ini pesanan saya? Saya cek dulu", "Pelanggan");
        StartCoroutine(ConfirmOrderWithDelay());
    }

    private IEnumerator ConfirmOrderWithDelay()
    {
        yield return new WaitForSeconds(2f);

        bool isCorrect = CheckOrder();
        if (isCorrect)
        {
            uiManager.ShowText("Wah, pesanan saya sudah benar! Terima kasih!", "Pelanggan");
            player.addMoney(10);
            player.addStress(-5);
        }
        else
        {
            uiManager.ShowText("Ini bukan pesanan saya! Saya kecewa!", "Pelanggan");
            uiManager.ShowLinneAngry();
            uiManager.HideNpcPortraitCenter();
            uiManager.ShowNpcPortraitCenterAngry();
            player.addStress(10);
        }

        yield return new WaitForSeconds(2f);
        FinishOrder();
    }

    private bool CheckOrder()
    {
        foreach (var item in customerOrder)
        {
            if (!playerOrder.ContainsKey(item.Key) || playerOrder[item.Key] != item.Value)
            {
                return false;
            }
        }
        return true;
    }

    private void ShowAddedItem(string item)
    {
        uiManager.ShowText($"Sudah memasukkan {item}.", "Kasir");
    }

    private void FinishOrder()
    {
        StartCoroutine(FinishOrderWithDelay());
    }

    protected override void EndActivity()
    {
        isOrdering = false;
        isProcessingActivity = false;
    }

    private IEnumerator FinishOrderWithDelay()
    {
        uiManager.ShowText("Pesanan selesai!", "Kasir");
        yield return new WaitForSeconds(3f);
        gameManager.SwitchToMainView();
        uiManager.HideMenuUI();
        uiManager.HideText();
        uiManager.HideRestaurantBackground();
        gameManager.AdvanceTime();
        EndActivity();
        uiManager.ShowLinneNormal();
    }
}
