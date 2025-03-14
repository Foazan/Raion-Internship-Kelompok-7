using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody rb;
    private SpriteRenderer sr;
    [SerializeField] private float Stress=0;
    [SerializeField] private float Hunger=100;
    [SerializeField] private float Sleep=100;
    [SerializeField] private float Money = 1000;
    private Animator animator;
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
    }
}
