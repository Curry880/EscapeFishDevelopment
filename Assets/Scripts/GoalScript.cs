using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalScript : MonoBehaviour
{
    private Animator shipAnimator;
    private GameManager gameManager;
    private void Start()
    {
        shipAnimator = GameObject.Find("HunterBoat").GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("spown goal");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("hit");
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player hit");
            shipAnimator.SetBool("isEnd", true);
            StartCoroutine(gameManager.EndGame());
        }
    }
}
