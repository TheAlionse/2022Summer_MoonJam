using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBossFight : MonoBehaviour
{
    public int health;
    public float max_static_size;
    public float smoothTime;

    public float bolt_force;
    public GameObject lightning_ring_prefab;
    public GameObject bolt_attack_prefab;
    public GameObject lightning_rods_prefab;
    public GameObject lightning_connections_prefab;


    private Renderer my_ren;

    private bool am_immune;
    private int phase = 2;
    private int phase_change1;
    private int phase_change2;

    private float currentVelocity = 0.0f;
    private float cur_scale_target = 0;
    private bool bigger = false;

    private void Start()
    {
        am_immune = false;
        my_ren = my_ren = gameObject.GetComponent<Renderer>();
        StartCoroutine("LightningFight");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet") && !am_immune)
        {
            StartCoroutine("dmgImmune");
            //update hp
            health -= collision.GetComponent<PlayerBulletDMG>().give_dmg();
            //change color?
            StartCoroutine("amred");
            //give temp immune?
            //play audio
            //update phase
            if (health <= phase_change1 && phase == 1)
            {
                Debug.Log("phase 2");
                phase = 2;
            }
            else if (health <= phase_change2 && phase == 2)
            {
                Debug.Log("phase 3");
                phase = 3;
            }
            else if (health <= 0)
            {
                Debug.Log("phase 3");
                //play death animation
                Destroy(gameObject);
            }
            Debug.Log(health);
        }
    }

    IEnumerator dmgImmune()
    {
        am_immune = true;
        yield return new WaitForSeconds(.5f);
        am_immune = false;
    }

    IEnumerator amred()
    {
        my_ren.material.color = new Color(.5f, 0f, 0f, 1f);
        yield return new WaitForSeconds(.2f);
        my_ren.material.color = new Color(1f, 1f, 1f, 1f);
    }

    private void FixedUpdate()
    {
        //movement

        float cur_scale = Mathf.SmoothDamp(lightning_ring_prefab.transform.localScale.x, cur_scale_target, ref currentVelocity, smoothTime);
        lightning_ring_prefab.transform.localScale = new Vector3(cur_scale, cur_scale, 1);
        if (Mathf.Abs(cur_scale_target - cur_scale) < .1f)
        {
            if (bigger)
            {
                cur_scale_target = 0f;
            }
            else
                cur_scale_target = max_static_size;
            bigger = !bigger;
        }
    }

    IEnumerator LightningFight()
    {
        if(phase == 1)
        {
            //bolt attack
            boltattack();
            yield return new WaitForSeconds(5f);
        }
        else if(phase == 2){
            //lightning rods
            lightningrods();
            yield return new WaitForSeconds(4f);
        }
        else if (phase == 3)
        {
            //connected to lightning rods
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("LightningFight");
    }

    private void lookat(Vector3 player_pos, Transform my_trans)
    {
        Vector2 look_dir = player_pos - my_trans.position;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
        my_trans.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    public void boltattack()
    {
        Vector3 player_pos = GameObject.FindWithTag("Player").transform.position;
        GameObject bolts = Instantiate(bolt_attack_prefab);
        //TODO: Add small margin of random
        lookat(player_pos, bolts.transform);
        bolts.GetComponent<Rigidbody2D>().AddForce((player_pos - bolts.transform.position) * bolt_force);
    }
    public void lightningrods()
    {
        Debug.Log("rod");
        //Bounds
        //X -202 -234 
        //Y 270 245
        float x_rng = Random.Range(-235, -202);
        float y_rng = Random.Range(245, 270);
        GameObject rods = Instantiate(lightning_rods_prefab);
        rods.transform.position = gameObject.transform.position;
        //give bolts rng pos
        rods.GetComponent<RodMovement>().giveCoord(x_rng, y_rng);
    }
}
