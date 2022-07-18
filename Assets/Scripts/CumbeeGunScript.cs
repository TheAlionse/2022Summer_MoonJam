using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeGunScript : MonoBehaviour
{

    public GameObject cumbee_bullet;
    public GameObject bulletSpawn;
    public GunStats gunStats;

    //public int max_concurrent_bees;
    //public float duration = 5;
    //public float velocity = 8;

    int current_bee_count;

    public void Shoot()
    {
        if (current_bee_count <= gunStats.max_bullet_count)
        {
            StartCoroutine(ShootFunction());
        }
    }

    IEnumerator ShootFunction()
    {
        current_bee_count++;
        GameObject bullet = Instantiate(cumbee_bullet, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletStats>().damage = gunStats.damage;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * gunStats.velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(gunStats.range_or_duration);

        Destroy(bullet);
    }

    void BeeDestroyed()
    {
        current_bee_count--;
    }
}
