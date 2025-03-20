using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    private UI_Manager uiManager;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isTalking = false;
    private Player player;
    private float playerStress;
    private Vector3 spawnLocation;
    private bool isWalking;
    private int decision;
    private int duration;
    [SerializeField] private int radiusLimit;
    [SerializeField] private GameObject rightLimit, leftLimit;
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        spawnLocation = transform.position;
        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.ShowInteractMessage();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            uiManager.HideInteractMessage();
        }
    }

    void Update()
    {
        if (!isWalking)
        {
            MakeDecision();
            
            StartCoroutine(Moving(duration));
        }
        if (transform.position.x >= rightLimit.transform.position.x || transform.position.x >= leftLimit.transform.position.x )
        {
            StopCoroutine(Moving(duration));
            decision = 0;
            StartCoroutine(Moving(duration));
        }
    }

    public void Interact()
    {
        if (isTalking) return;
        uiManager.HideInteractMessage();
        isTalking = true;
        StartDialogue();
    }

    private void StartDialogue()
    {
        if (player != null)
        {
            float PlayerStress = player.getStress();

            if (PlayerStress > 20)
            {
                uiManager.ShowLinneGloomy();
            }
            else
            {
                uiManager.ShowLinneNormal();
            }
        }
        uiManager.ShowText("Who are you?", "Old Man");
        

        uiManager.ShowText("Keep your own business", "Old Man");
        

        uiManager.ShowText("Oh, I am sorry", "Linne");

        StartCoroutine(EndDialogue());
    }

    IEnumerator EndDialogue()
    {
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        uiManager.HideText();
        isTalking = false;
        uiManager.ShowLinneNormal();
    }

    private void MakeDecision()
    {
        decision = UnityEngine.Random.Range(-1, 2); 
        duration = UnityEngine.Random.Range(2, 5); 
    }

    private IEnumerator Moving(float duration)
    {
        isWalking = true;
        float timePassed = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + new Vector3(decision *duration * 2f, 0f, 0f); 
        if(decision != 0)
        {
            if(decision == 1)
            {
                spriteRenderer.flipX = false;
            }
            else
            {
                spriteRenderer.flipX = true;
            }
            animator.SetBool("isWalking", true);
            while (timePassed < duration)
                {
                    timePassed += Time.deltaTime;
                    float t = timePassed / duration;
                    transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                    animator.SetFloat("inputx", decision);
                    yield return null;
                }
        }
        else
        {
            yield return new WaitForSeconds(duration); 
        }

            animator.SetBool("isWalking", false);
            isWalking = false;
            yield return new WaitForSeconds(1f);
    }
}