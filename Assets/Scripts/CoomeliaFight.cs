using System.Collections;
using UnityEngine;

public class CoomeliaFight : MonoBehaviour
{
    public float health;
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

    private bool am_immune;

    private int phase_change1;
    private int phase_change2;

    // Start is called before the first frame update
    void Start()
    {
        am_immune = false;
        phase_change1 = (int)(health * .6);
        phase_change2 = (int)(health * .3);
        move = true;
        stay = true;
        phase = 1;
        cur_move = new Vector2(left_x, min_height);
        facing_right = true;
        my_ren = gameObject.GetComponent<Renderer>();
        my_sprite = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine("FightRotation");
    }

    
    //TODO: UPDATE TO HANDLE WATERGUN
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
        if (move)
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
        if(phase == 2)
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
        if (phase == 3)
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
        GameObject warning = Instantiate(aurorabeam_warning_prefab.gameObject, transform);
        lookat(player_pos, warning.transform);
        yield return new WaitForSeconds(1f);
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
        GameObject charge = Instantiate(futuresight_charge_prefab, transform);
        if (!facing_right)
        {
            charge.GetComponent<SpriteRenderer>().flipX = true;
            xval = future_rightx_coord;
        }

        int yval = future_yposs[Random.Range(0, 3)];

        yield return new WaitForSeconds(2f);
        //maybe make fade in?
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
            Vector3 player_pos = GameObject.FindWithTag("Player").transform.position;
        GameObject moonblast = Instantiate(moonblast_prefab);
        moonblast.transform.position = firepoint.transform.position;
        lookat(player_pos, moonblast.transform);
        moonblast.GetComponent<Rigidbody2D>().AddForce((player_pos - moonblast.transform.position)* moonblast_force);
    }

}