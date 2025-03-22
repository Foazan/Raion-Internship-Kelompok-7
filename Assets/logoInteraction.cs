using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;
public class LogoInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactLogo;
    [SerializeField] private GameObject Enter;
    [SerializeField] private GameObject Arrow;
    
    void Start()
    {
        if(interactLogo != null)
        {
            interactLogo.SetActive(false);
        }
        if (Enter != null)
        {
            Enter.SetActive(false);
            Arrow.SetActive(false);
        }
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (interactLogo != null)
        {
            interactLogo.SetActive(true);
        }
        if (Enter != null)
        {
            Enter.SetActive(true);
            Arrow.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Enter != null)
        {
            Enter.transform.position = new Vector3((other.transform.position.x + 3f), other.transform.position.y + 1f, other.transform.position.z);
            Arrow.transform.position = new Vector3((other.transform.position.x + 1f), Enter.transform.position.y, other.transform.position.z);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (interactLogo != null)
        {
            interactLogo.SetActive(false);
        }
        if (Enter != null)
        {
            Enter.SetActive(false);
            Arrow.SetActive(false);
        }
    }
}
