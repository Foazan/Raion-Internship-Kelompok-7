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
    [SerializeField] private float minusHunger;
    [SerializeField] private float addedSleep;

    private string[] menuItems =
    {
        "Burger", "Pizza", "Soda", "Fried Chicken", "Noodles",
        "Coffee", "Ice Cream", "Salad", "Steak"
    };

    private Player player;

    protected override void Start()
    {
        base.Start();
        activityName = "Serving Customer";
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

    public override void StartActivity()
    {
        SoundManager.Instance.PlayDoorOpening();
        gameManager.SwitchToRestaurantView();
        uiManager.ShowRestaurantBackground();
        uiManager.HideInteractMessage();
        ShowRestaurantOptions();
    }

    private void ShowRestaurantOptions()
    {
        uiManager.ShowText("What do you want to do", "Cashier");
        uiManager.ShowText("1. Work \n2. Buy food \nchoose number and klik space", "Cashier");

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
            else if (Input.GetKeyDown(KeyCode.Alpha2) && gameManager.currentTime != "Siang")
            {
                optionSelected = true;
                BuyFood();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && gameManager.currentTime == "Siang")
            {
                uiManager.ShowText("I have to work", "Linne");
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1) && (gameManager.currentTime != "Siang" || gameManager.currentTime != "Pagi"))
            {
                uiManager.ShowText("Sorry you can't work right now", "Cashier");
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

        uiManager.ShowText($"{activityName}...", "Cashier");
        uiManager.ShowText($"I am ordering {GetTotalOrderCount()} item.", "Customer");

        ShowOrderSummary();
        ShowMenu();
    }

    private void BuyFood()
    {

        if (player.getMoney() >= foodCost)
        {
            player.addMoney(-foodCost);
            player.addHunger(addedHunger);
            uiManager.ShowText("Ah this is gonna put a dent in my pocket… \nbut it’s nice to eat good food every once a while.", "Linne");
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
        uiManager.ShowText($"I am ordering:", "Customer");
        foreach (var item in customerOrder)
        {
            uiManager.ShowText($"- {item.Key} x{item.Value}", "Customer");
        }
    }

    private void ShowMenu()
    {
        uiManager.ShowMenuUI();
        uiManager.ShowText("Select the menus.", "Cashier");
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
            uiManager.ShowText("Anything else? press space if order completed.", "Cashier");
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
        uiManager.ShowText("Is this my order?", "Customer");
        StartCoroutine(ConfirmOrderWithDelay());
    }

    private IEnumerator ConfirmOrderWithDelay()
    {
        yield return new WaitForSeconds(2f);

        bool isCorrect = CheckOrder();
        if (isCorrect)
        {
            uiManager.ShowRandomNPCDialogueRestaurant(true);
            player.addMoney(addedMoney);
            player.addStress(addedStress);
            player.addMoney(addedMoney);
        }
        else
        {
            uiManager.ShowRandomNPCDialogueRestaurant(false);
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
        uiManager.ShowText($"{item} selected.", "Cashier");
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
        uiManager.ShowText("Order completed!", "Cashier");
        yield return new WaitForSeconds(5f);
        gameManager.SwitchToMainView();
        uiManager.HideMenuUI();
        uiManager.HideText();
        uiManager.HideRestaurantBackground();
        gameManager.AdvanceTime();
        EndActivity();
        uiManager.ShowLinneNormal();
        uiManager.UpdateMoneyText();
        player.addSleep(addedSleep);
        player.addHunger(minusHunger);

        if (currentDay == 1)
        {
            uiManager.ShowText("*Gasp* That was mortifying. \nHow do people regularly interact with other people regularly? \nMy heart goes crazy and it gets hard to breathe!", "Linne");
            uiManager.ShowText("...but I want things to change. I have to hold on, at least for a week.", "Linne");
            uiManager.ShowText("I could go home now, but there’s still some time. \nMaybe I can still go somewhere. \nI never really did see around this street even though I live here.", "Linne");

        }
    }
}
