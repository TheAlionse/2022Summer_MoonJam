using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBossFight : MonoBehaviour
{
    public int health;
    public float shadowball_force;
    public float speed;
    public GameObject shadowball_prefab;

    public AudioSource shadow_ball_audio;
    public AudioSource damage_audio;
    public AudioSource spawn_audio;
    public AudioSource phase_change_audio;
    public AudioSource die_audio;

    private Renderer my_ren;

    private bool am_immune;
    private int phase = 1;
    private int phase_change1;
    private bool move_circle;
    //x 444.5 //y 33, 9.5
    //x 432.5 456.5 //y 21.5
    Vector2[] move_points = new Vector2[] { new Vector2(444.5f, 33), new Vector2(456.5f, 21.5f), new Vector2(444.5f, 9.5f), new Vector2(432, 21.5f)};

    private Vector2 my_move_point;
    private int move_point_count;
    private GameObject player;

    private void Start()
    {
        phase_change1 = (int)(health * .5);
        am_immune = false;
        my_ren = my_ren = gameObject.GetComponent<Renderer>();
        StartCoroutine("DarkFight");
        move_point_count = 0;
        my_move_point = move_points[move_point_count];
        move_circle = true;
        player = GameObject.FindWithTag("Player");
    }

    void OnEnable()
    {
        spawn_audio.Play();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerProjectile") && !am_immune)
        {
            StartCoroutine("dmgImmune");
            //update hp
            damage_audio.Play();
            health -= (int)collision.GetComponent<BulletStats>().damage;
            //change color?
            StartCoroutine("amred");
            //give temp immune?
            //play audio
            //update phase
            if (health <= phase_change1 && phase == 1)
            {
                Debug.Log("phase 2");
                phase_change_audio.Play();
                phase = 2;
                speed += 2;
            }
            else if (health <= 0)
            {
                Debug.Log("phase 3");
                //play death animation
                die_audio.Play();
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

    public void FixedUpdate()
    {
        if (move_circle) {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, my_move_point, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, my_move_point) < 0.001f)
            {
                Debug.Log("too close");
                if (move_point_count >= 3)
                {
                    move_point_count = 0;
                }
                else
                    move_point_count++;
                my_move_point = move_points[move_point_count];
            }
        }
        else
        {
            Debug.Log("moving at player");
            Vector2 player_pos = player.transform.position;
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player_pos, (1 + speed) * Time.deltaTime);
        }
    }


    IEnumerator DarkFight()
    {
        yield return new WaitForSeconds(2f);
        //Shadow ball attack
        ShadowBall();
        yield return new WaitForSeconds(1f);
        if (phase == 2)
        {
            //Run at player
            move_circle = false;
            yield return new WaitForSeconds(2f);
            move_circle = true;
            yield return new WaitForSeconds(1f);
        }
        StartCoroutine("DarkFight");
    }
    private void lookat(Vector3 player_pos, Transform my_trans)
    {
        Vector2 look_dir = player_pos - my_trans.position;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
        my_trans.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void ShadowBall()
    {
        shadow_ball_audio.Play();
        Vector2 player_pos = player.transform.position;
        GameObject shadowball = Instantiate(shadowball_prefab);
        shadowball.transform.position = gameObject.transform.position;
        lookat(player_pos, shadowball.transform);
        shadowball.GetComponent<Rigidbody2D>().AddForce((player_pos - (Vector2)shadowball.transform.position) * shadowball_force);
    }
}
