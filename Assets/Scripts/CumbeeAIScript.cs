using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeAIScript : MonoBehaviour
{
    public GunStats gunStats;
    public List<GameObject> points;
    public SpriteRenderer spriteRenderer;
    public float speed;
    public int health = 100;

    public GameObject spawn_gun;

    bool looking_right = true;
    Rigidbody2D rb;
    Vector3 target;
    Vector3 initialScale;
    // Start is called before the first frame update
    void Start()
    {
        points.Add(GameObject.FindGameObjectsWithTag("Player")[0]);
        rb = gameObject.GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
        InvokeRepeating("Shoot", 2f, gunStats.cooldown);
        InvokeRepeating("Move", 2f, 2f);
    }

    void Shoot()
    {
        gameObject.SendMessage("AIShoot");
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            float step = speed * Time.deltaTime;

            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }



    }

    private void FixedUpdate()
    {
        var delta = GameObject.FindGameObjectsWithTag("Player")[0].transform.position - transform.position;
        if (delta.x >= 0 && !looking_right)
        {
            transform.localScale = initialScale;
            looking_right = true;
        }
        else if (delta.x < 0 && looking_right)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            looking_right = false;
        }
    }

    void Move()
    {
        target = points[Random.Range(0, points.Count)].transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            int damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            if (health <= 0)
            {
                spawn_gun.transform.position = gameObject.transform.position;
                spawn_gun.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterBeam")
        {
            int damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            if (health <= 0)
            {
                spawn_gun.transform.position = gameObject.transform.position;
                spawn_gun.SetActive(true);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator TakeDamage()
    {
        spriteRenderer.color = new Color(255, 0, 0);

        yield return new WaitForSeconds(0.3f);

        spriteRenderer.color = new Color(255, 255, 255);
    }
}
