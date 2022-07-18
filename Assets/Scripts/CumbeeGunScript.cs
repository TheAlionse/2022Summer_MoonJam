using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeGunScript : MonoBehaviour
{

    public GameObject cumbee_bullet;
    public GameObject bulletSpawn;
    public GunStats gunStats;

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

    public void AIShoot()
    {
        if (current_bee_count <= gunStats.max_bullet_count)
        {
            StartCoroutine(AIShootFunction());
        }
    }

    IEnumerator AIShootFunction()
    {
        current_bee_count++;
        GameObject bullet = Instantiate(cumbee_bullet, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletStats>().damage = gunStats.damage;
        Vector2 target = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * gunStats.velocity;
        bullet.transform.localScale = bullet.transform.localScale * gunStats.size;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(gunStats.range_or_duration);

        Destroy(bullet);
    }
}
