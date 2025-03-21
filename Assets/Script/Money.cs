using UnityEngine;
public class Money : MonoBehaviour
{
    [SerializeField] float value;
    [SerializeField] Transform ChangeLocation;
    private Vector3 OriginalPosition;
    private Quaternion OriginalRotation;
    public GameObject Duit;
    public GameObject Minimarket;
    private Vector3 OriginalScale;
    private bool child;
    private bool isMoving = false;

    private void Start()
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation;
        OriginalScale = transform.localScale;
        
    }

    private void Update()
    {
        if (isMoving)
        {
            moneyMovement();
        }

    }

    private void OnMouseDown()
    {
        isMoving = true;
    }
    public float getValue()
    {
        return value;
    }
    private void moneyMovement()
    {
        if (transform.position == OriginalPosition || isMoving && !child)
        {
            if (transform.position == OriginalPosition)
            {
                Instantiate(Duit, transform.position, OriginalRotation);
                Minimarket.GetComponent<MinimarketActivity>().addChange(value);
                
            }
            float step = 2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ChangeLocation.position, step);

            if (transform.position == ChangeLocation.position)
            {
                    transform.SetParent(GameObject.Find("changeTray").transform);
               
                isMoving = false;
                child = true;
            }
        }
        else if (child)
        {
            if (transform.position == ChangeLocation.position)
            {
                Minimarket.GetComponent<MinimarketActivity>().subtractChange(value);
            }
            float step = 2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, OriginalPosition, step);
            
            if (Vector3.Distance(transform.position, OriginalPosition) < 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}