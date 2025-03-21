using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEditor;
using Unity.VisualScripting;
public class LogoInteraction : MonoBehaviour
{
    [SerializeField] private GameObject interactLogo;
    
    void Start()
    {
        interactLogo.SetActive(false);
    }

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        interactLogo.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        interactLogo.SetActive(false);
    }
}
