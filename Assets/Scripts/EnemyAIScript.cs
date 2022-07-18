using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    public float speed;
    public float health;
    public float damage;
    public SpriteRenderer spriteRenderer;
    bool looking_right = true;
    Vector3 initialScale;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        initialScale = transform.localScale;
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PlayerProjectile")
        {
            float damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "WaterBeam")
        {
            float damage = collision.gameObject.GetComponent<BulletStats>().damage;
            health -= damage;
            StartCoroutine(TakeDamage());
            if (health <= 0)
            {
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
