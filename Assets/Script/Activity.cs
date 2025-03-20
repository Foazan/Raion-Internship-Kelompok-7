using UnityEngine;

public abstract class Activity : MonoBehaviour
{
    protected GameManager gameManager;
    protected UI_Manager uiManager;
    protected bool isInActivityZone = false;
    protected bool isProcessingActivity = false; 
    protected string activityName = "Aktivitas";
    

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    protected virtual void Update()
    {
        if (isInActivityZone && Input.GetKeyDown(KeyCode.E) && !isProcessingActivity)
        {
            isProcessingActivity = true; 
            Debug.Log($"Memulai {activityName}...");
            StartActivity();
        }
    }

    protected virtual void StartActivity()
    {
        uiManager.HideInteractMessage();
    }

    protected virtual void EndActivity()
    {
        isProcessingActivity = false; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInActivityZone)
        {
            isInActivityZone = true;
            Debug.Log($"Tekan SPACE untuk memulai {activityName}");
        }

        uiManager.ShowInteractMessage();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInActivityZone = false;
        }

        uiManager.HideInteractMessage();
    }
}
