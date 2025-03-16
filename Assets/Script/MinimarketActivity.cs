using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MinimarketActivity : Activity
{
    private Dictionary<string, float> customerOrder = new Dictionary<string, float>();
    private float totalAmount = 0f; 
    private float paymentAmount = 0f; 
    private float changeAmount = 0f;
    private TextMeshProUGUI CashierText;


    private string[] listBarang =
    {
        "Snack", "Tissue", "Sabun", "Minuman", "Roti",
        "Sampo", "Pasta Gigi", "Sabun Cuci", "Baterai", "Kopi",
        "Teh", "Gula", "Garam", "Minyak Goreng", "Kecap",
        "Saus", "Mie Instan", "Biskuit", "Coklat", "Susu"
    };
    private float[] listUang =
    {
        0.50f,1f,2f,5f,10f,20f,50f,100f
    };

    protected override void Start()
    {
        base.Start();
        activityName = "Melayani Pembeli";
        CashierText = GameObject.Find("ScreenUI").GetComponent<TextMeshProUGUI>();
        StartActivity();
    }

    protected override void Update()
    {
        
    }

    protected override void StartActivity()
    {
        customerOrder.Clear();
        totalAmount = 0f; 
        Debug.Log($"Memulai {activityName}...");

        GenerateRandomOrder();
        GenerateRandomPayment(totalAmount);
        UpdateTotalDisplay();
        Debug.Log("Dia Bayar " + paymentAmount + " leee");
    }

    private void GenerateRandomOrder()
    {
        int numberOfItems = Random.Range(1, 9); //random 1-8 barang
        for (int i = 0; i < numberOfItems; i++)
        {
            string randomItem = listBarang[Random.Range(0, listBarang.Length)];
            int quantity = Random.Range(1, 4); // jumlah 1-3

            float price = GetItemPrice(randomItem);
            if (price > 0)
            {
                if (!customerOrder.ContainsKey(randomItem))
                    customerOrder[randomItem] = 0;

                customerOrder[randomItem] += price * quantity; // Update total item
                totalAmount += price * quantity; // Update total harga
                UpdateTotalDisplay();
                Debug.Log($"Ditambahkan: {randomItem} - Harga: {price} x {quantity}. Total: {totalAmount}");
            }
        }
    }

    private float GetItemPrice(string item)
    {
        switch (item)
        {
            case "Snack": return 1.50f;
            case "Tissue": return 0.50f;
            case "Sabun": return 1.00f;
            case "Minuman": return 1.20f;
            case "Roti": return 0.80f;
            case "Sampo": return 2.00f;
            case "Pasta Gigi": return 1.50f;
            case "Sabun Cuci": return 1.00f;
            case "Baterai": return 2.50f;
            case "Kopi": return 1.80f;
            case "Teh": return 1.00f;
            case "Gula": return 0.70f;
            case "Garam": return 0.60f;
            case "Minyak Goreng": return 3.00f;
            case "Kecap": return 1.20f;
            case "Saus": return 1.50f;
            case "Mie Instan": return 0.90f;
            case "Biskuit": return 1.00f;
            case "Coklat": return 1.50f;
            case "Susu": return 1.20f;
            default: return 0f;
        }
    }

    private void GenerateRandomPayment(float price)
    {
        int i = listUang.Length;
        while (paymentAmount <= price )
            {
            if (price - paymentAmount < listUang[i-1])
            {
                paymentAmount += listUang[Random.Range(2, i )];
                if (i < 4) {
                    i--;
                }
                
            }

            }
    }

    public void GiveChangeDone()
    {
        changeAmount = paymentAmount - totalAmount;
        if (changeAmount != paymentAmount - totalAmount) 
        {
            Debug.Log("Salah bodo");
            FinishTransaction();
        }
        else
        {
            Debug.Log("Anjay bener");
            FinishTransaction();
        }
    
    }
    private void FinishTransaction()
    {
        totalAmount = 0f;
        paymentAmount = 0f;
        changeAmount = 0f;
        customerOrder.Clear();
        UpdateTotalDisplay();
        StartActivity();
    }

    private void UpdateTotalDisplay()
    {
        CashierText.text = $"Total     : {totalAmount:F2}\n" +
                           $"Pembayaran: {paymentAmount:F2}\n" +
                           $"Kembalian : {changeAmount:F2}";
    }
    public void addChange(float a)
    {
        changeAmount += a;
    }

    public void subtractChange(float a)
    {
        changeAmount -= a;
    }
}