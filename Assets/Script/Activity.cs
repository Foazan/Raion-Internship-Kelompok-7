using UnityEngine;

public abstract class Activity : MonoBehaviour
{
    protected GameManager gameManager;
    protected bool isInActivityZone = false;
    protected bool isProcessingActivity = false; 
    protected string activityName = "Aktivitas";

    protected virtual void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    protected virtual void Update()
    {
        if (isInActivityZone && Input.GetKeyDown(KeyCode.Space) && !isProcessingActivity)
        {
            isProcessingActivity = true; 
            Debug.Log($"Memulai {activityName}...");
            StartActivity();
        }
    }

    protected abstract void StartActivity();

    protected void EndActivity()
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
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInActivityZone = false;
        }
    }
}
