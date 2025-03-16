using UnityEngine;
using System.Collections.Generic;

public class Money : MonoBehaviour
{
    [SerializeField] float value;
    [SerializeField] Transform ChangeLocation;
    private Vector3 OriginalPosition;
    private Quaternion OriginalRotation;
    public GameObject Duit;

    private bool child;

    private bool isMoving = false; 

    private void Start()
    {
        OriginalPosition = transform.position;
        OriginalRotation = transform.rotation; 
    }

    private void Update()
    {
        if(isMoving == true)
        {
            moneyMovement();
        }
    }

    private void OnMouseDown()
    {
        isMoving = true;
    }
    private void moneyMovement()
    {
        if (transform.position == OriginalPosition || isMoving == true && !child)
        {
            if(transform.position == OriginalPosition)
            {
                Instantiate(Duit, transform.position, OriginalRotation);
            }
            float step = 0.2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, ChangeLocation.position, step);

            if (transform.position == ChangeLocation.position)
            {
                transform.SetParent(GameObject.Find("Kasir").transform);
                isMoving = false;
                child = true;
            }
        }
        else if (child)
        {
            float step = 0.2f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, OriginalPosition, step);
            if (Vector3.Distance(transform.position, OriginalPosition) < 0.01f)
            {
                Destroy(gameObject);
            }
        }
    }
}