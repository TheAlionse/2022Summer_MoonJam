using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    public float speed;
    public int health;
    public int damage;
    public SpriteRenderer spriteRenderer;
    GameObject spawnManager;
    bool looking_right = true;
    Vector3 initialScale;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        spawnManager = GameObject.FindGameObjectWithTag("SpawnManager");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        var delta = player.transform.position - transform.position;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            int damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            if (health <= 0)
            {
                spawnManager.GetComponent<SpawnerManager>().Decrement();
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
                spawnManager.GetComponent<SpawnerManager>().Decrement();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().takeDamage(damage);
        }
    }
}
