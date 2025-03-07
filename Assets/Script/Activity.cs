using UnityEngine;

public class Activity : MonoBehaviour
{
    public string activityName = "Aktivitas"; 
    public float activityDuration = 5f; 

    private bool isInActivityZone = false;
    private bool isDoingActivity = false;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (isInActivityZone && !isDoingActivity && Input.GetKeyDown(KeyCode.Space))
        {
            StartActivity();
        }
    }

    void StartActivity()
    {
        isDoingActivity = true;
        Debug.Log(activityName + " dimulai...");
        Invoke("FinishActivity", activityDuration);
    }

    void FinishActivity()
    {
        isDoingActivity = false;
        Debug.Log(activityName + " selesai!");
        gameManager.AdvanceTime(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInActivityZone = true;
            Debug.Log("Dekat dengan " + activityName);
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
