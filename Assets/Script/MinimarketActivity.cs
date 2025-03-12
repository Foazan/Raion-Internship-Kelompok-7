using System.Collections.Generic;
using UnityEngine;

public class MinimarketActivity : Activity
{
    Dictionary<string, float> customerOrder = new Dictionary<string, float>();
    Dictionary<string,float> screenOrder = new Dictionary<string, float>();
    private bool isScanning = false;
    private bool isGivingChange = false;
    private bool screenDisplayedOrder = false;
    private string[] listBarang =
    {
    "Snack", "Tissue", "Sabun", "Minuman", "Roti",
    "Sampo", "Pasta Gigi", "Sabun Cuci", "Baterai", "Kopi",
    "Teh", "Gula", "Garam", "Minyak Goreng", "Kecap",
    "Saus", "Mie Instan", "Biskuit", "Coklat", "Susu"
    };
    protected override void Start()
    {
        base.Start();
        activityName = "Melayani Pembeli";
    }
    protected override void Update()
    {
        base.Update();
        if (!isScanning) return;
        if (isGivingChange) return;

        for (int i = 0; i < listBarang.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                ScanItem(listBarang[i]);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return) && !screenDisplayedOrder)
        {
            DisplayOrder();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            GiveChange();
        }
    }
    protected override void StartActivity()
    {
        if (isScanning) return;

        isScanning = true;
        customerOrder.Clear();
        screenOrder.Clear();
        screenDisplayedOrder = false;

        Debug.Log($"Memulai {activityName}...");
    }

    private void ScanItem(string item)
    {
        float price = GetItemPrice(item);
        if (price > 0)
        {
            if (!customerOrder.ContainsKey(item))
                customerOrder[item] = 0;

            customerOrder[item] += price;
            Debug.Log($"Ditambahkan: {item} - Harga: {price}. Total: {customerOrder[item]}");

            if (!screenOrder.ContainsKey(item))
                screenOrder[item] = 0;

            screenOrder[item] += price;
        }
        else
        {
            Debug.Log($"Item {item} tidak ditemukan.");
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

    private void DisplayOrder()
    {
        if (!screenDisplayedOrder)
        {
            Debug.Log("Pesanan pelanggan:");
            foreach (var item in screenOrder)
            {
                Debug.Log($"- {item.Key}: {item.Value}");
            }
            screenDisplayedOrder = true;
            Debug.Log("Tekan 'G' untuk memberikan kembalian.");
        }
    }

    private void GiveChange()
    {
        if (!screenDisplayedOrder) return;

        Debug.Log("Memberikan kembalian kepada pelanggan...");
        isGivingChange = true;

        Invoke("FinishTransaction", 2f); 
    }

    private void FinishTransaction()
    {
        isGivingChange = false;
        isScanning = false;
        screenDisplayedOrder = false;
        customerOrder.Clear();
        screenOrder.Clear();
        Debug.Log("Transaksi selesai! Terima kasih telah berbelanja.");
    }
}
