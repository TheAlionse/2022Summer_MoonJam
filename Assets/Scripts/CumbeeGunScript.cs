using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeGunScript : MonoBehaviour
{

    public GameObject cumbee_bullet;
    public GameObject bulletSpawn;

    public float duration = 5;

    public float velocity = 8;

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
}
