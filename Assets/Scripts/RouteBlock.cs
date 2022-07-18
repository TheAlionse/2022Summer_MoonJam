using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class RouteBlock : MonoBehaviour
{
    public string DialogTitle;
    private DialogueRunner dialogue_runner;
    public bool isBoss = false;
    private void Start()
    {
        dialogue_runner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //maybe lock playermovement later
        dialogue_runner.StartDialogue(DialogTitle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isBoss)
        {
            collision.GetComponent<PlayerHealth>().SetBossTrigger(gameObject);
        }
        dialogue_runner.StartDialogue(DialogTitle);
        gameObject.SetActive(false);
    }
}
