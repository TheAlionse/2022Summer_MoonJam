using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeGunScript : MonoBehaviour
{

    public GameObject cumbee_bullet;
    public GameObject bulletSpawn;
    public int max_concurrent_bees;
    public float duration = 5;
    public float velocity = 8;
    public float cooldown;

    int current_bee_count;
    float cooldown_timestamp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && current_bee_count <= max_concurrent_bees && cooldown_timestamp <= Time.time)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        cooldown_timestamp = Time.time + cooldown;
        current_bee_count++;
        GameObject bullet = Instantiate(cumbee_bullet, bulletSpawn.transform.position, Quaternion.identity);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(duration);

        Destroy(bullet);
    }

    void BeeDestroyed()
    {
        current_bee_count--;
    }
}
