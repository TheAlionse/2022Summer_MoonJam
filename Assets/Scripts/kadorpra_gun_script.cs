using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kadorpra_gun_script : MonoBehaviour
{
    public GameObject spoon;
    public GameObject bulletSpawn;
    public float velocity = 15;
    public float size = 2;
    public float range = 1;
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
        GameObject bullet = Instantiate(spoon, bulletSpawn.transform.position, Quaternion.identity);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        direction.Normalize();

        bullet.GetComponent<Rigidbody2D>().velocity = direction * velocity;
        bullet.GetComponent<Rigidbody2D>().angularVelocity = -1500;
        bullet.transform.localScale = bullet.transform.localScale * size;

        yield return new WaitForSeconds(range);

        Destroy(bullet);
    }
}

