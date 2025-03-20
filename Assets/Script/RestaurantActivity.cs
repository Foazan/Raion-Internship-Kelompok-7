using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class RestaurantActivity : Activity
{
    private Dictionary<string, int> playerOrder = new Dictionary<string, int>();
    private Dictionary<string, int> customerOrder = new Dictionary<string, int>();
    private bool isOrdering = false;
    private System.Random random = new System.Random();
    [SerializeField] private float addedStress;
    [SerializeField] private float addedMoney;
    [SerializeField] private float foodCost;
    [SerializeField] private float addedHunger;

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
        uiManager.ShowRestaurantBackground();
        uiManager.HideInteractMessage();
        ShowRestaurantOptions();
    }

    private void ShowRestaurantOptions()
    {
        uiManager.ShowText("Apa yang ingin Anda lakukan?", "Kasir");
        uiManager.ShowText("1. Bekerja di restoran \n2. Beli makan \npilih angka dan klik space", "Kasir");

        StartCoroutine(WaitForOptionSelection());
    }

    private IEnumerator WaitForOptionSelection()
    {
        bool optionSelected = false;

        while (!optionSelected)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && (gameManager.currentTime == "Siang" || gameManager.currentTime == "Pagi"))
            {
                optionSelected = true;
                StartWork();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                optionSelected = true;
                BuyFood();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && (gameManager.currentTime != "Siang" || gameManager.currentTime != "Pagi"))
            {
                uiManager.ShowText("Maaf Anda tidak bisa bekerja saat ini", "Kasir");
                StartCoroutine(EndAfterNotDoingAnything());
                optionSelected = true;
            }
            yield return null;
        }
    }

    private void StartWork()
    {
        isOrdering = true;
        playerOrder.Clear();
        customerOrder.Clear();
        uiManager.ShowRestaurantBackground();
        uiManager.ShowNpcPortraitCenterNormal();
        GenerateRandomOrder();

        uiManager.ShowText($"Memulai {activityName}...", "Kasir");
        uiManager.ShowText($"Saya akan memesan {GetTotalOrderCount()} item.", "Pelanggan");

        ShowOrderSummary();
        ShowMenu();
    }

    private void BuyFood()
    {

        if (player.getMoney() >= foodCost)
        {
            player.addMoney(-foodCost);
            player.addHunger(addedHunger);
            uiManager.ShowText("Terimakasih sudah membeli", "Kasir");
        }
        else
        {
            uiManager.ShowText("Uang Anda tidak cukup untuk membeli makanan!", "Kasir");
        }

        StartCoroutine(EndAfterPurchase());
    }

    private IEnumerator EndAfterPurchase()
    {
        yield return new WaitForSeconds(3f);
        gameManager.SwitchToMainView();
        uiManager.HideText();
        uiManager.HideRestaurantBackground();
        gameManager.AdvanceTime();
        EndActivity();
        uiManager.ShowLinneNormal();
        uiManager.UpdateMoneyText();
    }

    private IEnumerator EndAfterNotDoingAnything()
    {
        yield return new WaitForSeconds(3f);
        gameManager.SwitchToMainView();
        uiManager.HideText();
        uiManager.HideRestaurantBackground();
        EndActivity();
        uiManager.ShowLinneNormal();
        uiManager.UpdateMoneyText();
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
            player.addMoney(addedMoney);
            player.addStress(addedStress);
            player.addMoney(addedMoney);
        }
        else
        {
            uiManager.ShowText("Ini bukan pesanan saya! Saya kecewa!", "Pelanggan");
            uiManager.ShowLinneGloomy();
            uiManager.HideNpcPortraitCenter();
            uiManager.ShowNpcPotraitCenterAngry();
            player.addStress(addedStress+20);
            player.addMoney(addedMoney);
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
        uiManager.UpdateMoneyText();
    }
}
