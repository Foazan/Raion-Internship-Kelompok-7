using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private SpriteRenderer sr;
    private UI_Manager uiManager;
    private GameManager gameManager;
    [SerializeField] private float Stress=0;
    [SerializeField] private float Hunger=100;
    [SerializeField] private float Sleep=100;
    [SerializeField] private float Money = 1000;
    private Animator animator;
    private bool isNearNpc = false;
    public Vector3 lastValidPosition;
    public float getStress()
    {
        return Stress;
    }

    public float getMoney()
    {
        return Money;
    }

    public float getHunger()
    {
        return Hunger;
    }

    public float getSleep()
    {
        return Sleep;
    }

    public void addStress(float Stress)
    {
        this.Stress += Stress;
    }
    public void addHunger(float Hunger)
    {
        this.Hunger += Hunger;
    }
    public void addSleep(float Sleep)
    {
        this.Sleep += Sleep;
    }

    public void addMoney(float Money)
    {
        this.Money += Money;
    }
    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void Update()
    {
        float x = Input.GetAxis("Horizontal");
        Vector3 moveDir = new Vector3(x, 0, 0);
        rb.linearVelocity = moveDir * speed;
        if (x != 0 && x < 0)
        {
            sr.flipX = true;
            animator.SetBool("isWalking", true);
            animator.SetFloat("input.-x", x);
        }
        else if (x != 0 && x > 0)
        {
            sr.flipX = false;
            animator.SetBool("isWalking", true);
            animator.SetFloat("input.x", x);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        CheckForNpcInteraction();

        if (isNearNpc && Input.GetKeyDown(KeyCode.E))
        {
            InteractWithNpc();
        }
    }

    private void FixedUpdate()
    {
        
        lastValidPosition = transform.position;
        
    }


    private void CheckForNpcInteraction()
    {
        float interactRange = 1f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        bool foundNpc = false;

        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NpcManager npcManager))
            {
                foundNpc = true;
                break;
            }
        }

        if (foundNpc && !isNearNpc)
        {
            isNearNpc = true;
            uiManager.ShowInteractMessage();
        }
        else if (!foundNpc && isNearNpc)
        {
            isNearNpc = false;
            uiManager.HideInteractMessage();
        }
    }

    private void InteractWithNpc()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, 1f);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out NpcManager npcManager))
            {
                npcManager.Interact();
                break;
            }
        }
    }
}
