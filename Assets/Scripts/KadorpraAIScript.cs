using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KadorpraAIScript : MonoBehaviour
{
    public GunStats gunStats;
    public List<GameObject> points;
    public SpriteRenderer spriteRenderer;
    public float speed;
    public float health = 100;

    Rigidbody2D rb;
    Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        points.Add(GameObject.FindGameObjectsWithTag("Player")[0]);
        rb = gameObject.GetComponent<Rigidbody2D>();
        InvokeRepeating("Shoot", 2.0f, gunStats.cooldown);
        InvokeRepeating("Move", 2.0f, 2.0f);
    }

    void Shoot()
    {
        gameObject.SendMessage("AIShoot");
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            float step = speed * Time.deltaTime;
            
            transform.position = Vector3.MoveTowards(transform.position, target, step);
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
            Debug.Log("hit");
            float damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            Debug.Log(health);
            if (health <= 0)
            {
                Destroy(gameObject);
                Debug.Log("kadropra dead");
            }
        }
    }

    IEnumerator TakeDamage()
    {
        spriteRenderer.color = new Color(255,0,0);

        yield return new WaitForSeconds(0.3f);

        spriteRenderer.color = new Color(255, 255, 255);
    }


}
