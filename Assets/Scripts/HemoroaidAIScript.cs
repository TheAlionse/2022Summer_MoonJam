using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemoroaidAIScript : MonoBehaviour
{
    public GameObject waterBeam;
    public GameObject bulletSpawn;
    public GunStats gunStats;
    public GameObject[] targets;
    public SpriteRenderer spriteRenderer;
    public GameObject spawn_gun;

    public AudioSource shoot_audio;

    public float angularSpeed;
    public float frenzyAngularSpeed;
    public float frenzyAttackVelocity;
    public int frenzyAttacksAmount;
    public float frenzyAttackDuration;
    public float frenzyAttackCooldown;
    public float normalShootDuration;
    public float normalShootCooldown;
    public int health;

    bool looking_right = false;
    Vector3 initialScale;
    GameObject waterBeamInstance = null;
    bool shooting = false;
    float current_speed;
    bool frenzy_attack = false;
    GameObject target;
    bool charging = false;
    float current_velocity;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectsWithTag("Player")[0];
        current_speed = angularSpeed;
        current_velocity = gunStats.velocity;
        initialScale = transform.localScale;
        StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);

        while (true)
        {
            
            Debug.Log("shoot start");
            target = GameObject.FindGameObjectsWithTag("Player")[0];

            for (int i = 0; i < 3; i++)
            {
                shooting = false;
                yield return new WaitForSeconds(normalShootCooldown);

                shooting = true;

                yield return new WaitForSeconds(normalShootDuration);
            }

            shooting = false;

            //frenzy attack
            frenzy_attack = true;
            charging = true;
            target = targets[Random.Range(0, targets.Length)];
            current_speed = frenzyAngularSpeed;
            current_velocity = frenzyAttackVelocity;

            yield return new WaitForSeconds(2f);

            charging = false;

            for (int i = 0; i < frenzyAttacksAmount - 1; i++)
            {
                yield return new WaitForSeconds(frenzyAttackCooldown);

                shooting = true;

                yield return new WaitForSeconds(frenzyAttackDuration);

                shooting = false;
                GameObject[] gos = RandomClosestTargetToPlayer();
                GameObject new_target = gos[Random.Range(0, 3)];
                while (new_target.transform.position == target.transform.position)
                {
                    new_target = gos[Random.Range(0, 3)];
                }

                target = new_target;
            }

            yield return new WaitForSeconds(frenzyAttackCooldown);

            shooting = true;

            yield return new WaitForSeconds(frenzyAttackDuration);

            shooting = false;
            frenzy_attack = false;
            current_velocity = gunStats.velocity;
            current_speed = angularSpeed;

        }
    }

    IEnumerator FrenzyAttack()
    {
        frenzy_attack = true;
        charging = true;
        target = targets[Random.Range(0, targets.Length)];
        current_speed = frenzyAngularSpeed;
        current_velocity = frenzyAttackVelocity;

        yield return new WaitForSeconds(2f);

        charging = false;

        for(int i = 0; i < frenzyAttacksAmount-1; i++)
        {
            yield return new WaitForSeconds(0.2f);

            shooting = true;

            yield return new WaitForSeconds(0.3f);

            shooting = false;
            GameObject[] gos = RandomClosestTargetToPlayer();
            GameObject new_target = gos[Random.Range(0, 3)];
            while (new_target.transform.position == target.transform.position)
            {
                new_target = gos[Random.Range(0, 3)];
            }

            target = new_target;
        }

        yield return new WaitForSeconds(0.2f);

        shooting = true;

        yield return new WaitForSeconds(0.3f);

        shooting = false;
        frenzy_attack = false;

    }

    public GameObject[] RandomClosestTargetToPlayer()
    {
        GameObject[] gos = new GameObject[3];
        Vector3 position = GameObject.FindGameObjectsWithTag("Player")[0].transform.position;
        float distance1 = Mathf.Infinity;
        float distance2 = Mathf.Infinity;
        float distance3 = Mathf.Infinity;

        foreach (GameObject go in targets)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance1)
            {
                distance3 = distance2;
                distance2 = distance1;
                distance1 = curDistance;
                gos[2] = gos[1];
                gos[1] = gos[0];
                gos[0] = go;
            }
            else if (curDistance < distance2)
            {
                distance3 = distance2;
                distance2 = curDistance;
                gos[2] = gos[1];
                gos[1] = go;
            }
            else if (curDistance < distance3)
            {
                distance3 = curDistance;
                gos[2] = go;
            }
        }
        return gos;
    }

    // Update is called once per frame
    void Update()
    {
        if (waterBeamInstance == null && shooting)
        {
            shoot_audio.Play();
            ShootWaterBeam();
        }

        if (waterBeamInstance != null && !shooting)
        {
            shoot_audio.Stop();
            Destroy(waterBeamInstance);
            waterBeamInstance = null;
        }
    }

    private void FixedUpdate()
    {
        if (!charging)
        {
            if (transform.rotation.eulerAngles.z > 90 && transform.rotation.eulerAngles.z < 270)
            {
                transform.localScale = new Vector3(initialScale.x, -initialScale.y, initialScale.z);
            }
            else
            {
                transform.localScale = initialScale;
            }

            Vector2 look_dir = target.transform.position - gameObject.transform.position;
            float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;

            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, Time.deltaTime * current_speed);
        }

    }

    public void ShootWaterBeam()
    {
        waterBeamInstance = Instantiate(waterBeam, bulletSpawn.transform.position, gameObject.transform.rotation);
        waterBeamInstance.transform.parent = gameObject.transform;
        waterBeamInstance.GetComponent<BulletStats>().damage = gunStats.damage;
        waterBeamInstance.GetComponent<WaterBeamScript>().max_length = gunStats.range_or_duration;
        waterBeamInstance.GetComponent<WaterBeamScript>().speed = current_velocity;
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
