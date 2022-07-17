using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoomdraGunScript : MonoBehaviour
{
    public GameObject[] cooms;
    public GameObject bulletSpawn;

    public float velocity = 25;
    public float range = 0.25f;
    public float size = 1;
    public int bullet_count = 6;
    public float spread = 80f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {

        GameObject[] bullets = new GameObject[bullet_count];
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        for (int i = 0; i < bullet_count; i++)
        {
            GameObject coom = cooms[Random.Range(0, cooms.Length)];
            GameObject bullet = Instantiate(coom, bulletSpawn.transform.position, Quaternion.identity);
            Vector2 random_direction = Rotate(direction, Random.Range(-spread / 2, spread / 2));
            bullet.GetComponent<Rigidbody2D>().velocity = random_direction * velocity;
            bullet.transform.localScale = bullet.transform.localScale * size;
            float angle = Mathf.Atan2(random_direction.y, random_direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bullets[i] = bullet;
        }




        yield return new WaitForSeconds(range);

        for(int i = 0; i < bullet_count; i++)
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
