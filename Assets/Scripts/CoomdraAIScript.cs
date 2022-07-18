using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoomdraAIScript : MonoBehaviour
{
    public GunStats gunStats;
    public SpriteRenderer spriteRenderer;
    public float speed;
    public float chargeSpeed;
    public float chargeDelay;
    public float chargeDuration;
    public int health = 100;
    public float chargeCooldown;

    public GameObject spawn_gun;

    bool looking_right = true;
    Rigidbody2D rb;
    Vector3 target;
    Vector3 initialScale;
    GameObject player;
    float current_speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        initialScale = transform.localScale;
        StartCoroutine(Shoot());
        rb = gameObject.GetComponent<Rigidbody2D>();
        InvokeRepeating("Charge", 2f, chargeCooldown+chargeDelay+chargeDuration);

    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            if (current_speed != 0)
            {
                gameObject.SendMessage("AIShoot");
            }
            yield return new WaitForSeconds(gunStats.cooldown);


        }
    }

    // Update is called once per frame
    void Update()
    {
            float step = current_speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
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

    void Charge()
    {
        StartCoroutine(ChargeUp());
    }

    IEnumerator ChargeUp()
    {
        current_speed = 0;

        yield return new WaitForSeconds(chargeDelay);

        current_speed = chargeSpeed;

        yield return new WaitForSeconds(chargeDuration);

        current_speed = speed;
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
