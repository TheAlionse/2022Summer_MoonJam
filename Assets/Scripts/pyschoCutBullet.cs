using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pyschoCutBullet : MonoBehaviour
{
    public int dmg;
    private float timer;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 8f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerHealth>().takeDamage(dmg);
            Destroy(gameObject);
        }
    }
}
