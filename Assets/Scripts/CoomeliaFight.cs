using System.Collections;
using UnityEngine;

public class CoomeliaFight : MonoBehaviour
{
    public int max_health;
    public float speed;
    public float pyscho_cut_force; 
    public float moonblast_force;

    public float future_leftx_coord;
    public float future_rightx_coord;
    public int[] future_yposs = new int[] { -25, -40, -55 };

    public GameObject pyschocut_prefab;
    public GameObject aurorabeam_warning_prefab;
    public GameObject aurorabeam_hit_prefab;
    public GameObject futuresight_warning_prefab;
    public GameObject futuresight_hit_prefab;
    public GameObject futuresight_post_prefab;
    public GameObject futuresight_charge_prefab;
    public GameObject firepoint;
    public GameObject moonblast_prefab;
    public GameObject cave_exit;
    public GameObject route_block;
    public GameObject legendgun;
    private static GameObject bosshp_bar;

    public AudioSource charging_beam_audio;
    public AudioSource switch_sides_audio;
    public AudioSource firing_beam_audio;
    public AudioSource blade_storm_audio;
    public AudioSource aoe_charge_audio;
    public AudioSource aoe_cast_audio;
    public AudioSource run_billy_run;
    public AudioSource die_audio;
    public AudioSource take_damage_audio;
    public AudioSource spawn_audio;

    private Vector2 cur_move;
    private float max_height = -25;
    private float min_height = -55;
    private float left_x = 170;
    private float right_x = 217;
    private bool facing_right;
    private SpriteRenderer my_sprite;
    private bool psychocut_offcycle = false;
    private int phase;
    private bool move;
    private bool stay;
    private Renderer my_ren;

    private float currentVelocity;
    private float smoothTime = 2f;
    private bool am_immune;
    private bool kill_me;

    private int phase_change1;
    private int phase_change2;

    private int health;

    // Start is called before the first frame update
    void Start()
    {
        am_immune = false;
        phase_change1 = (int)(health * .7);
        phase_change2 = (int)(health * .4);
        move = true;
        stay = true;
        phase = 1;
        cur_move = new Vector2(left_x, min_height);
        facing_right = true;
        my_ren = gameObject.GetComponent<Renderer>();
        my_sprite = gameObject.GetComponent<SpriteRenderer>();
        
    }

    void OnEnable()
    {
        am_immune = false;
        move = true;
        stay = true;
        spawn_audio.Play();
        health = max_health;
        phase = 1;
        bosshp_bar = GameObject.FindGameObjectWithTag("BOSSHPUI");
        bosshp_bar.GetComponent<BossHealthBar>().enableroot();
        bosshp_bar.GetComponent<BossHealthBar>().SetHealth(health, max_health);
        StartCoroutine("FightRotation");
    }

    
    //TODO: UPDATE TO HANDLE WATERGUN
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile") && !am_immune)
        {
            StartCoroutine("dmgImmune");
            //update hp
            take_damage_audio.Play();
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
                Debug.Log("ded");
                die_audio.Play();
                cave_exit.SetActive(true);
                route_block.SetActive(false);
                //play death animation
                move = false;
                am_immune = true;
                StopCoroutine("FightRotation");
                legendgun.SetActive(true);
                kill_me = true;
                bosshp_bar.GetComponent<BossHealthBar>().disableroot();
                GameObject.FindWithTag("Player").GetComponent<PlayerHealth>().removeBossTrigger();
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
        if (move && !kill_me)
        {
            transform.position = Vector3.MoveTowards(gameObject.transform.position, cur_move, speed * Time.deltaTime);
            if (Vector3.Distance(gameObject.transform.position, cur_move) < 0.01f)
            {
                if (cur_move.y == max_height)
                {
                    cur_move.y = min_height;
                }
                else
                {
                    cur_move.y = max_height;
                }
            }
        }
        else if (kill_me)
        {
            float cur_scale = Mathf.SmoothDamp(gameObject.transform.localScale.x, 0, ref currentVelocity, smoothTime);
            gameObject.transform.localScale = new Vector3(cur_scale, cur_scale, 1);
            if (cur_scale < .001f)
            {

                Destroy(gameObject);
            }
        }
    }

    IEnumerator FightRotation()
    {
        yield return new WaitForSeconds(2f);
        if (phase == 1) {
            //psycho cut //lots of blades
            psychocut();
            yield return new WaitForSeconds(1f);
            //pyscho cut
            psychocut();
            yield return new WaitForSeconds(1f);

            //aurora beam // large beam
            StartCoroutine("aurorabeam");
            yield return new WaitForSeconds(5f);
        }
        //phase 2
        else if(phase == 2)
        {
            //double team //spawn fake that also uses abilities low hp(transition)
            //aurora beam
            StartCoroutine("aurorabeam");
            yield return new WaitForSeconds(5f);
            //pyscho cut
            psychocut();
            yield return new WaitForSeconds(1f);
            //Future Sight Charge //waits
            StartCoroutine("futuresight");
            yield return new WaitForSeconds(1f);
            //pyscho cut
            psychocut();
            yield return new WaitForSeconds(1f);
            //aurora beam
            StartCoroutine("aurorabeam");
            yield return new WaitForSeconds(5f);
        }

        //phase 3
        else if (phase == 3)
        {
            //double team(transition)
            //moon beam (transition) (heal)
            //moon blast //Large Balls
            moonblast();
            yield return new WaitForSeconds(1f);
            //Future Sight Charge
            StartCoroutine("futuresight");
            yield return new WaitForSeconds(1f);
            //aurora beam
            StartCoroutine("aurorabeam");
            yield return new WaitForSeconds(5f);
            //aurora beam
            StartCoroutine("aurorabeam");
            yield return new WaitForSeconds(5f);
            //Future Sight Hit (AOE)
            //pyscho cut
            psychocut();
            yield return new WaitForSeconds(1f);
            psychocut();
            yield return new WaitForSeconds(1f);
            psychocut();
            yield return new WaitForSeconds(1f);
        }

        //tp to other side
        yield return new WaitForSeconds(1f);
        if (!stay)
        {
            switchsides();
        }
        stay = !stay;
        StartCoroutine("FightRotation");
    }

    private void switchsides()
    {
        switch_sides_audio.Play();
        float x_val = 0;
        if (facing_right)
            x_val = right_x;
        else
            x_val = left_x;
        gameObject.transform.position = new Vector2(x_val, gameObject.transform.position.y);
        cur_move = new Vector2(x_val, cur_move.y);
        my_sprite.flipX = !my_sprite.flipX;
        facing_right = !facing_right;
    }

    public void AddForceAtAngle(float force, float angle, Rigidbody2D rb)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;

        rb.AddForce(new Vector2(xcomponent, ycomponent));
    }

    private void psychocut()
    {
        blade_storm_audio.Play();
        int angle_offcycle = 0;
        if (psychocut_offcycle)
        {
            angle_offcycle = 16;
        }
        for (int i = 0; i <= 5; ++i)
        {
            float offset;
            GameObject cut_obj = Instantiate(pyschocut_prefab);
            cut_obj.transform.position = firepoint.transform.position;
            //Vector2 directionVector;
            if (facing_right)
            {
                offset = -90f;
                //directionVector = Vector2.right;
            }
            else
            {
                offset = 90f;
                //directionVector = Vector2.left;
                //cut_obj.GetComponent<SpriteRenderer>().flipX = true;
            }
            AddForceAtAngle(pyscho_cut_force, (36 * i) + offset - angle_offcycle, cut_obj.GetComponent<Rigidbody2D>());
            cut_obj.transform.rotation = Quaternion.Euler(0, 0, (36 * i) + offset - angle_offcycle);
        }
        psychocut_offcycle = !psychocut_offcycle;
    }

    private void lookat(Vector3 player_pos, Transform my_trans)
    {
        Vector2 look_dir = player_pos - my_trans.position;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
        my_trans.rotation = Quaternion.Euler(0f, 0f, angle);
    }
    IEnumerator aurorabeam()
    {
        move = false;
        Vector3 player_pos = GameObject.FindWithTag("Player").transform.position;
        charging_beam_audio.Play();
        GameObject warning = Instantiate(aurorabeam_warning_prefab.gameObject, transform);
        lookat(player_pos, warning.transform);
        yield return new WaitForSeconds(1f);
        firing_beam_audio.Play();
        GameObject beam = Instantiate(aurorabeam_hit_prefab, transform);
        Destroy(warning);
        lookat(player_pos, beam.transform);
        yield return new WaitForSeconds(2f);
        Destroy(beam);
        move = true;
    }

    IEnumerator futuresight()
    {
        float xval = future_leftx_coord;
        aoe_charge_audio.Play();
        GameObject charge = Instantiate(futuresight_charge_prefab, transform);
        if (!facing_right)
        {
            charge.GetComponent<SpriteRenderer>().flipX = true;
            xval = future_rightx_coord;
        }

        int yval = future_yposs[Random.Range(0, 3)];

        yield return new WaitForSeconds(2f);
        //maybe make fade in?
        aoe_cast_audio.Play();
        GameObject warning = Instantiate(futuresight_warning_prefab);
        warning.transform.position = new Vector2(xval, yval);
        yield return new WaitForSeconds(2f);
        GameObject hit = Instantiate(futuresight_hit_prefab);
        hit.transform.position = new Vector2(xval, yval);
        Destroy(warning);
        Destroy(charge);
        yield return new WaitForSeconds(2f);
        GameObject post = Instantiate(futuresight_post_prefab);
        post.transform.position = new Vector2(xval, yval);
        Destroy(hit);
        yield return new WaitForSeconds(2f);
        Destroy(post);
    }

    private void moonblast() //dmg
    {
        run_billy_run.Play();
        Vector3 player_pos = GameObject.FindWithTag("Player").transform.position;
        GameObject moonblast = Instantiate(moonblast_prefab);
        moonblast.transform.position = firepoint.transform.position;
        lookat(player_pos, moonblast.transform);
        moonblast.GetComponent<Rigidbody2D>().AddForce((player_pos - moonblast.transform.position)* moonblast_force);
    }

}
