using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && tag == "EnemyProjectile")
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(gameObject.GetComponent<BulletStats>().damage);
        }

        if((collision.gameObject.tag == "Enemy" && tag == "PlayerProjectile") || (collision.gameObject.tag == "Player" && tag == "EnemyProjectile"))
        {
            Destroy(gameObject);
        }
    }
}
