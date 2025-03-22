using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class MinimarketActivity : Activity
{
    private bool lancar;
    private Dictionary<string, float> customerOrder = new Dictionary<string, float>();
    private float totalAmount = 0f; 
    private float paymentAmount = 0f; 
    private float changeAmount = 0f;
    [SerializeField]private TextMeshProUGUI CashierText;
    private bool transactionFinished = false;
    private Player player;
    private int banyakCustomer;
    [SerializeField] private float addedStress;
    [SerializeField] private GameObject NPC;
    [SerializeField] private GameObject MidPoint, EndPoint;
    private Vector3 NpcStartPosition;

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
        NpcStartPosition = NPC.transform.position;
        banyakCustomer = Random.Range(2, 5);
        activityName = "Melayani Pembeli";
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        UpdateTotalDisplay();
        StartActivity();
    }
    protected override void Update()
    {
        base.Update();
    }

    
    public override void StartActivity()
    {
        gameManager.SwitchtoMinimarketView();
        ContinueActivity();
    }

    private void ContinueActivity()
    {
        StartCoroutine(NpcComing());
    }

    private IEnumerator GenerateRandomOrder()
    {
        int numberOfItems = Random.Range(3, 9); //random 1-8 barang
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
                yield return new WaitForSeconds(1f);
                Debug.Log($"Ditambahkan: {randomItem} - Harga: {price} x {quantity}. Total: {totalAmount}");
            }
        }
        GenerateRandomPayment(totalAmount);
        UpdateTotalDisplay();
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
        paymentAmount += listUang[Random.Range(2, i)];
        while (paymentAmount < price)
        {
            if (i <= 0) break; 

            if (price - paymentAmount <= listUang[i - 1])
            {
                if (i <= 3)
                {
                    paymentAmount += listUang[Random.Range(2, i)];
                }
                else
                {
                    i--;
                }

            }
            else
            {
                paymentAmount += listUang[Random.Range(2, i)];
            }
        }
    }
    public float RoundToPointTwo(float value)
    {
        return Mathf.Round(value / 0.2f) * 0.2f;
    }
    public void GiveChangeDone()
    {
        float expectedChange = paymentAmount - totalAmount;
        expectedChange = RoundToPointTwo(expectedChange);


        Debug.Log($"Change Amount: {changeAmount}");
        Debug.Log($"Payment Amount: {paymentAmount}");
        Debug.Log($"Total Amount: {totalAmount}");
        Debug.Log($"Expected Change: {expectedChange}");

        if (Mathf.Abs(changeAmount - expectedChange) > 0.01f)
        {
            lancar = false;
        }
        else
        {
            Debug.Log("Anjay bener");
        }
        SoundManager.Instance.PlayCashier();

        banyakCustomer--;
        StartCoroutine(FinishTransaction());
    }


    private IEnumerator FinishTransaction()
    {
        totalAmount = 0f;
        paymentAmount = 0f;
        changeAmount = 0f;
        customerOrder.Clear();
        clearChangeTray();
        UpdateTotalDisplay();
        yield return StartCoroutine(NpcLeaving());
        NPC.transform.position = NpcStartPosition;
        if (banyakCustomer != 0)
        {
            ContinueActivity();
        }
        else
        {

            EndActivity();

        }
        
    }

    protected override void EndActivity()
    {
        player.addMoney(100f);
        if (lancar && player.getStress() > 60)
        {
            player.addStress(60 + player.getStress());
        }
        else if (!lancar && player.getStress() > 80)
        {
            player.addStress(80 + player.getStress());
        }
            player.addSleep(-50);
            player.addHunger(-50);
        base.EndActivity();
        gameManager.SwitchToMainView();
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
        UpdateTotalDisplay();
    }

    public void subtractChange(float a)
    {
        changeAmount -= a;
        UpdateTotalDisplay();
    }
    private void clearChangeTray()
    {
        GameObject changeTray = GameObject.Find("changeTray");
       
        foreach (Transform child in changeTray.transform)
        {
            Destroy(child.gameObject);
        }

    }
    private void TransactionStart()
    { 
        customerOrder.Clear();
        totalAmount = 0f;
        Debug.Log($"Memulai {activityName}...");
        StartCoroutine(GenerateRandomOrder());
        UpdateTotalDisplay();
        
    }

    private IEnumerator NpcComing()
    {
        Debug.Log("StidaknyaSampeSini");
        float timeElapsed = 0f;
        while (NPC.transform.position != MidPoint.transform.position)
        {
            Debug.Log("NPCDATANGNPDATANG");
            NPC.transform.position = Vector3.Lerp(NpcStartPosition, MidPoint.transform.position, timeElapsed / 3);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        NPC.transform.position = MidPoint.transform.position;
        TransactionStart();

    }

    private IEnumerator NpcLeaving()
    {
        float timeElapsed = 0f;
        while (NPC.transform.position != EndPoint.transform.position)
        {
            NPC.transform.position = Vector3.Lerp(MidPoint.transform.position, EndPoint.transform.position, timeElapsed / 3);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        NPC.transform.position = EndPoint.transform.position;


    }

}