using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoomeliaGunScript : MonoBehaviour
{
    public GameObject moonBomb;
    public GameObject bulletSpawn;
    public AudioSource shoot_audio;
    public GunStats gunStats;

    PlayerMoveTest script;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        script = player.GetComponent<PlayerMoveTest>();
    }

    public void Shoot()
    {
        StartCoroutine(ShootFunction());
    }

    IEnumerator ShootFunction()
    {
        shoot_audio.Play();
        GameObject bullet = Instantiate(moonBomb, bulletSpawn.transform.position, Quaternion.identity);
        bullet.GetComponent<BulletStats>().damage = gunStats.damage;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * gunStats.velocity;
        bullet.transform.localScale = bullet.transform.localScale * gunStats.size;

        if (!script.looking_right)
        {
            bullet.transform.eulerAngles = new Vector3(0, 180, 0);
        }

        yield return new WaitForSeconds(gunStats.range_or_duration);

        Destroy(bullet);
    }
}
