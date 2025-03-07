using System;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField]
    private Transform teleportTarget;   
    private bool isPlayerInRange = false;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.Space))
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null && teleportTarget != null)
            {
                player.transform.position = teleportTarget.position;
          
            }
        }
    }
}
