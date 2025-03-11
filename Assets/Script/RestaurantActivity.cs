using UnityEngine;
using System.Collections.Generic;

public class RestaurantActivity : Activity
{
    private Dictionary<string, int> playerOrder = new Dictionary<string, int>();
    private Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private int currentOrderIndex = 0;
    private bool isOrdering = false;
    private bool hasDisplayedOrder = false;
    private System.Random random = new System.Random();

    private string[] menuItems =
    {
        "Burger", "Pizza", "Soda", "Fried Chicken", "Noodles",
        "Coffee", "Ice Cream", "Salad", "Steak"
    };

    protected override void Start()
    {
        base.Start();
        activityName = "Melayani Pelanggan";
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

        if (Input.GetKeyDown(KeyCode.Alpha0)) SkipItem();
        if (Input.GetKeyDown(KeyCode.Return) && currentOrderIndex >= customerOrder.Count) ConfirmOrder();
    }

    protected override void StartActivity()
    {
        if (isOrdering) return;

        isOrdering = true;
        currentOrderIndex = 0;
        playerOrder.Clear();
        customerOrder.Clear();
        hasDisplayedOrder = false;

        GenerateRandomOrder();

        Debug.Log($"Memulai {activityName}...");
        

        ShowNextOrderItem();
    }

    private void GenerateRandomOrder()
    {
        int orderCount = random.Next(2, 5);
        Debug.Log($"Pelanggan akan memesan {orderCount} item.");

        for (int i = 0; i < orderCount; i++)
        {
            string randomItem = menuItems[random.Next(menuItems.Length)];
            if (!customerOrder.ContainsKey(randomItem))
                customerOrder[randomItem] = 0;

            customerOrder[randomItem]++;
            Debug.Log($"Pesanan: {randomItem} x{customerOrder[randomItem]}");
        }
    }

    private void ShowNextOrderItem()
    {
        if (!hasDisplayedOrder)
        {
            Debug.Log($"Pesanan pelanggan: {customerOrder.Count} item");
            foreach (var item in customerOrder)
            {
                Debug.Log($"- {item.Key} x{item.Value}");
            }
            hasDisplayedOrder = true;
        }

        if (currentOrderIndex < customerOrder.Count)
        {
            ShowMenu();
        }
        else
        {
            Debug.Log("Anything else? Tekan Enter jika pesanan selesai.");
        }
    }

    private void ShowMenu()
    {
        Debug.Log("Pilih menu:");
        for (int i = 0; i < menuItems.Length; i++)
        {
            Debug.Log($"{i + 1}. {menuItems[i]}");
        }
        Debug.Log("Tekan angka 1-9 untuk memilih, atau 0 jika tidak ingin menambahkan.");
    }

    private void AddItem(string item)
    {
        if (!playerOrder.ContainsKey(item))
            playerOrder[item] = 0;

        playerOrder[item]++;
        Debug.Log($"Ditambahkan: {item}. Total {playerOrder[item]}");

        currentOrderIndex++;
        ShowNextOrderItem();
    }

    private void SkipItem()
    {
        Debug.Log($"Pelanggan: Baik, tidak jadi pesan {GetOrderItemAtIndex(currentOrderIndex)}.");
        currentOrderIndex++;
        ShowNextOrderItem();
    }

    private void ConfirmOrder()
    {
        Debug.Log("Pelanggan: Ini pesanan saya? Mari kita cek...");
        bool isCorrect = CheckOrder();

        if (isCorrect)
        {
            Debug.Log("Pelanggan: Wah, pesanan saya sudah benar! Terima kasih!");
        }
        else
        {
            Debug.Log("Pelanggan: Ini bukan pesanan saya! Saya kecewa!");
        }

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

    private void FinishOrder()
    {
        isOrdering = false;
        hasDisplayedOrder = false;
        Debug.Log("Pesanan selesai! Waktu dalam game maju.");

        gameManager.AdvanceTime();
    }

    private string GetOrderItemAtIndex(int index)
    {
        int count = 0;
        foreach (var item in customerOrder)
        {
            if (count == index)
                return item.Key;
            count++;
        }
        return null;
    }
}
