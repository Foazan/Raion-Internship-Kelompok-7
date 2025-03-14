using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RestaurantActivity : Activity
{
    private Dictionary<string, int> playerOrder = new Dictionary<string, int>();
    private Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private int currentOrderIndex = 0;
    private bool isOrdering = false;
    private System.Random random = new System.Random();
    private UI_Manager uiManager;

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
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
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

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SkipItem();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (currentOrderIndex >= customerOrder.Count) 
            {
                uiManager.ShowText("Anything else? Tekan spasi jika pesanan selesai.");
                ConfirmOrder();
            }
        }
    }



    protected override void StartActivity()
    {
        if (isOrdering) return;
        
        isOrdering = true;
        currentOrderIndex = 0;
        playerOrder.Clear();
        customerOrder.Clear();

        GenerateRandomOrder();
        uiManager.SwitchToRestaurantView();

        uiManager.ShowText($"Memulai {activityName}...");
        uiManager.ShowText($"Pelanggan akan memesan {customerOrder.Count} item.");

        ShowNextOrderItem();
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

    private void ShowNextOrderItem()
    {
        if (currentOrderIndex == 0)
        {
            uiManager.ShowText($"Pesanan pelanggan:");
            foreach (var item in customerOrder)
            {
                uiManager.ShowText($"- {item.Key} x{item.Value}");
            }
        }
        ShowMenu();
    }

    private void ShowMenu()
    {
        string menuMessage = "Pilih menu:\n";
        

        uiManager.ShowMenuUI();
        uiManager.ShowText(menuMessage);
    }

    private void AddItem(string item)
    {
        if (!playerOrder.ContainsKey(item))
            playerOrder[item] = 0;
        
        playerOrder[item]++;
        uiManager.Displaytext();
        currentOrderIndex++;
        ShowNextOrderItem();
    }

    private void SkipItem()
    {
        uiManager.ShowText($"Pelanggan: Baik, tidak jadi pesan {GetOrderItemAtIndex(currentOrderIndex)}.");
        currentOrderIndex++;
        ShowNextOrderItem();
    }

    private void ConfirmOrder()
    {
        uiManager.ShowText("Pelanggan: Ini pesanan saya? Mari kita cek...");
        bool isCorrect = CheckOrder();

        if (isCorrect)
        {
            uiManager.ShowText("Pelanggan: Wah, pesanan saya sudah benar! Terima kasih!");
            player.addMoney(10);
            player.addStress(-5);
        }
        else
        {
            uiManager.ShowText("Pelanggan: Ini bukan pesanan saya! Saya kecewa!");
            player.addStress(10);
        }

        StartCoroutine(FinishOrderWithDelay());
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
        
        uiManager.ShowText("Pesanan selesai!");
        uiManager.HideMenuUI();
        uiManager.SwitchToMainView();
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

    private IEnumerator FinishOrderWithDelay()
    {
        yield return new WaitForSeconds(10f); 
        FinishOrder();
    }
}
