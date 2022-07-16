using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadTestDMG : MonoBehaviour
{
    PlayerHealth player_health;

    private void Start()
    {
        player_health = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player_health.takeDamage(1);
        }
    }
}
