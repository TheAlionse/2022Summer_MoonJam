using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBossFight : MonoBehaviour
{
    public int max_health;
    public float max_static_size;
    public float smoothTime;

    public float bolt_force;
    public GameObject lightning_ring_prefab;
    public GameObject bolt_warning_prefab;
    public GameObject bolt_attack_prefab;
    public GameObject lightning_rods_prefab;
    public GameObject lightning_connections_prefab;
    public GameObject lightning_exit;
    public GameObject route_block;
    private static GameObject bosshp_bar;

    private Renderer my_ren;

    private bool am_immune;
    private int phase = 1;
    private int phase_change1;
    private int phase_change2;

    private float currentVelocity = 0.0f;
    private float cur_scale_target = 0;
    private bool bigger = false;

    private int maxCount = 3;
    private int curCount = 0;

    private bool kill_me = false;

    private int health;
    private void Start()
    {
        phase_change1 = (int)(health * .7);
        phase_change2 = (int)(health * .4);
        am_immune = false;
        my_ren = my_ren = gameObject.GetComponent<Renderer>();
    }

    void OnEnable()
    {
        //spawn_audio.Play();
        am_immune = false;
        health = max_health;
        phase = 1;
        bosshp_bar = GameObject.FindGameObjectWithTag("BOSSHPUI");
        bosshp_bar.GetComponent<BossHealthBar>().enableroot();
        bosshp_bar.GetComponent<BossHealthBar>().SetHealth(health, max_health);
        StartCoroutine("LightningFight");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile") || collision.CompareTag("WaterBeam") && !am_immune)
        {
            StartCoroutine("dmgImmune");
            //update hp
            health -= (int)collision.GetComponent<BulletStats>().damage;
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
                StopCoroutine("LightningFight");
                Debug.Log("boss ded");
                lightning_exit.SetActive(true);
                route_block.SetActive(false);
                //play death animation
                kill_me = true;
                GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().removeBossTrigger();
                bosshp_bar.GetComponent<BossHealthBar>().disableroot();
            }
            Debug.Log(health);
            bosshp_bar.GetComponent<BossHealthBar>().SetHealth(health, max_health);
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
        if (kill_me)
        {
            float cur_scale = Mathf.SmoothDamp(gameObject.transform.localScale.x, 0, ref currentVelocity, smoothTime);
            gameObject.transform.localScale = new Vector3(cur_scale, cur_scale, 1);
            if (cur_scale < .001f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
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
    }

    IEnumerator LightningFight()
    {
        curCount++;
        yield return new WaitForSeconds(2f);
        if (phase == 1)
        {
            //bolt attack
            StartCoroutine("boltattack");
            yield return new WaitForSeconds(2f);
        }
        else if(phase == 2){
            //lightning rods
            lightningrods();
            yield return new WaitForSeconds(2f);
            StartCoroutine("boltattack");
            yield return new WaitForSeconds(1f);
        }
        else if (phase == 3)
        {
            lightningrods();
            yield return new WaitForSeconds(1.5f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("LightningFight");

        if(curCount >= maxCount)
        {
            myMove();
            curCount = 0;
        }
    }

    private void lookat(Vector3 player_pos, Transform my_trans)
    {
        Vector2 look_dir = player_pos - my_trans.position;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
        my_trans.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator boltattack()
    {
        Vector3 player_pos = GameObject.FindWithTag("Player").transform.position;
        GameObject warning = Instantiate(bolt_warning_prefab.gameObject, transform);
        lookat(player_pos, warning.transform);
        yield return new WaitForSeconds(.75f);
        GameObject beam = Instantiate(bolt_attack_prefab, transform);
        Destroy(warning);
        lookat(player_pos, beam.transform);
        yield return new WaitForSeconds(1.5f);
        Destroy(beam);
    }

    private void myMove()
    {
        //X -202 -234 
        int[] xposs = new int[] { -202, -234 };
        //Y 270 245
        int[] yposs = new int[] {270, 245};
        float x_rng = xposs[Random.Range(0, 2)];
        float y_rng = yposs[Random.Range(0, 2)];
        gameObject.transform.position = new Vector2(x_rng, y_rng); 
    }

    public void lightningrods()
    {
        Debug.Log("rod");
        //Bounds
        //X -202 -234 
        //Y 270 245
        float x_rng = Random.Range(-235, -202);
        float y_rng = Random.Range(245, 270);
        GameObject rod = Instantiate(lightning_rods_prefab);
        rod.transform.position = gameObject.transform.position;
        //give bolts rng pos
        rod.GetComponent<RodMovement>().giveCoord(x_rng, y_rng, phase,  gameObject);
    }
}
