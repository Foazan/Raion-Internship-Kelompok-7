using System;
using Unity.Cinemachine;
using UnityEngine;

public class MultipleChoiceInteracUI : MonoBehaviour
{
    [SerializeField] private GameObject Menu1, Menu2, Arrow;
    private int pilihan=1;
    private bool keyIsdown;
    private bool canInteract;
    [SerializeField] private bool minimarket, restaurant;
    void Start()
    {
        Menu1.SetActive(false);
        Menu2.SetActive(false); 
        Arrow.SetActive(false);
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.W))&& !keyIsdown)
        {
            if (pilihan <= 1)
            {
                pilihan++;
            }
            else
            {
                pilihan--;
            }
            keyIsdown = true;
        }
        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W))
        {
            keyIsdown = false;
        }

        if(canInteract && Input.GetKeyDown(KeyCode.E) && pilihan == 1 && minimarket)
        {
            gameObject.GetComponent<MinimarketActivity>().StartActivity();
        }
        else if (canInteract && Input.GetKeyDown(KeyCode.E) && pilihan == 2 && minimarket)
        {
            gameObject.GetComponent<StatsActivity>().StartActivity();
        }

        if(canInteract && Input.GetKeyDown(KeyCode.E) && pilihan == 1 && restaurant)
        {
            gameObject.GetComponent<RestaurantActivity>().StartActivity();
        }
        if(canInteract && Input.GetKeyDown(KeyCode.E) && pilihan == 2 && restaurant)
        {
            gameObject.GetComponent<StatsActivity>().StartActivity();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Menu1.SetActive(true);
        Menu2.SetActive(true);
        Arrow.SetActive(true);
        pilihan = 1;
    }
    private void OnTriggerStay(Collider other)
    {
        canInteract = true;
        Menu1.transform.position = new Vector3((other.transform.position.x + 3f), other.transform.position.y+1f, other.transform.position.z);
        Menu2.transform.position = new Vector3((other.transform.position.x + 3f), other.transform.position.y, other.transform.position.z);
        if(pilihan == 1)
        {
            Arrow.transform.position = new Vector3((other.transform.position.x+1f),Menu1.transform.position.y,other.transform.position.z);
        }
        else
        {
            Arrow.transform.position = new Vector3((other.transform.position.x + 1f), Menu2.transform.position.y, other.transform.position.z);
        }

    }

    private void OnTriggerExit()
    {
        Menu1.SetActive(false);
        Menu2.SetActive(false);
        Arrow.SetActive(false);
        canInteract = false;
    }
}
