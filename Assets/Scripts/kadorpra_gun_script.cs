using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kadorpra_gun_script : MonoBehaviour
{
    public GameObject spoon;
    public GameObject bulletSpawn;
    public AudioSource shoot_audio;
    public GunStats gunStats;
    public float velocity = 15;
    public float size = 2;
    public float range = 1;

    public void Shoot()
    {
        StartCoroutine(ShootFunction());
    }

    IEnumerator ShootFunction()
    {
        shoot_audio.Play();
        GameObject bullet = Instantiate(spoon, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletStats>().damage = gunStats.damage;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * gunStats.velocity;
        bullet.GetComponent<Rigidbody2D>().angularVelocity = -1500;
        bullet.transform.localScale = bullet.transform.localScale * gunStats.size;

        yield return new WaitForSeconds(gunStats.range_or_duration);

        Destroy(bullet);
    }

    public void AIShoot()
    {
        StartCoroutine(AIShootFunction());
    }

    IEnumerator AIShootFunction()
    {
        GameObject bullet = Instantiate(spoon, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletStats>().damage = gunStats.damage;
        Vector2 target = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * gunStats.velocity;
        bullet.GetComponent<Rigidbody2D>().angularVelocity = -1500;
        bullet.transform.localScale = bullet.transform.localScale * gunStats.size;

        yield return new WaitForSeconds(gunStats.range_or_duration);

        Destroy(bullet);
    }
}

