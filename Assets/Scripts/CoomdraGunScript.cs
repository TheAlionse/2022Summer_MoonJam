using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoomdraGunScript : MonoBehaviour
{
    public GameObject[] cooms;
    public GameObject bulletSpawn;
    public GunStats gunStats;

    public void Shoot()
    {
        StartCoroutine(ShootFunction());
    }

    IEnumerator ShootFunction()
    {

        GameObject[] bullets = new GameObject[gunStats.max_bullet_count];
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        for (int i = 0; i < gunStats.max_bullet_count; i++)
        {
            GameObject coom = cooms[Random.Range(0, cooms.Length)];
            GameObject bullet = Instantiate(coom, bulletSpawn.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletStats>().damage = gunStats.damage;
            Vector2 random_direction = Rotate(direction, Random.Range(-gunStats.spread / 2, gunStats.spread / 2));
            bullet.GetComponent<Rigidbody2D>().velocity = random_direction * gunStats.velocity;
            bullet.transform.localScale = bullet.transform.localScale * gunStats.size;
            float angle = Mathf.Atan2(random_direction.y, random_direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bullets[i] = bullet;
        }




        yield return new WaitForSeconds(gunStats.range_or_duration);

        for(int i = 0; i < gunStats.max_bullet_count; i++)
        {
            Destroy(bullets[i]);
        }
    }

    public void AIShoot()
    {
        StartCoroutine(AIShootFunction());
    }

    IEnumerator AIShootFunction()
    {

        GameObject[] bullets = new GameObject[gunStats.max_bullet_count];
        Vector2 target = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        for (int i = 0; i < gunStats.max_bullet_count; i++)
        {
            GameObject coom = cooms[Random.Range(0, cooms.Length)];
            GameObject bullet = Instantiate(coom, bulletSpawn.transform.position, Quaternion.identity);
            bullet.GetComponent<BulletStats>().damage = gunStats.damage;
            Vector2 random_direction = Rotate(direction, Random.Range(-gunStats.spread / 2, gunStats.spread / 2));
            bullet.GetComponent<Rigidbody2D>().velocity = random_direction * gunStats.velocity;
            bullet.transform.localScale = bullet.transform.localScale * gunStats.size;
            float angle = Mathf.Atan2(random_direction.y, random_direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bullets[i] = bullet;
        }




        yield return new WaitForSeconds(gunStats.range_or_duration);

        for (int i = 0; i < gunStats.max_bullet_count; i++)
        {
            Destroy(bullets[i]);
        }
    }

    Vector2 Rotate(Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}
