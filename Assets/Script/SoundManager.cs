using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    
    [SerializeField] private AudioSource doorOpening;
    [SerializeField] private AudioSource playerWalking;
    [SerializeField] private AudioSource Cashier;
    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayDoorOpening()
    {
        doorOpening.Play();
    }

    public void PlayPlayerWalking()
    {
        if (playerWalking != null && !playerWalking.isPlaying)
        {
            playerWalking.loop = true;
            playerWalking.Play();
        }
    }

    public void StopPlayerWalking()
    {
        if (playerWalking != null && playerWalking.isPlaying)
        {
            playerWalking.loop = false;
            playerWalking.Stop();
        }
    }

    public void PlayCashier()
    {
        if (Cashier != null) Cashier.Play();
    }
}
