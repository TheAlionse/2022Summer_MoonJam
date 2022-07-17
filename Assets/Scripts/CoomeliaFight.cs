using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoomeliaFight : MonoBehaviour
{
    public float health;
    public float speed;
    public float pyscho_cut_force;

    public GameObject pyschocut_prefab;
    public GameObject aurorabeam_warning_prefab;
    public GameObject aurorabeam_hit_prefab;
    public GameObject futuresight_warning_prefab;
    public GameObject futuresight_hit_prefab;
    public GameObject futuresight_post_prefab;
    public GameObject futresight_charge_prefab;
    public GameObject firepoint;


    private Vector2 cur_move;
    private float max_height = -25;
    private float min_height = -55;
    private float left_x = 170;
    private float right_x = 217;
    private bool facing_right;
    private SpriteRenderer my_sprite;
    // Start is called before the first frame update
    void Start()
    {
        cur_move = new Vector2(left_x, min_height);
        facing_right = true;
        my_sprite = gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine("FightRotation");
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(gameObject.transform.position, cur_move, speed * Time.deltaTime);
        if(Vector3.Distance(gameObject.transform.position, cur_move) < 0.01f)
        {
            if(cur_move.y == max_height)
            {
                cur_move.y = min_height;
            }
            else
            {
                cur_move.y = max_height;
            }
        }
    }
    IEnumerator FightRotation()
    {
        yield return new WaitForSeconds(2f);
        psychocut();
        yield return new WaitForSeconds(2f);
        //psycho cut //lots of blades
        //pyscho cut
        //aurora beam // large beam
        //tp to other side

        //phase 2
        //double team //spawn fake that also uses abilities low hp(transition)
        //aurora beam
        //pyscho cut
        //Future Sight Charge
        //pyscho cut
        //aurora beam
        //Future Sight Hit (AOE)
        //tp to other side

        //phase 3
        //double team(transition)
        //moon beam (transition) (heal)
        //moon blast //Large Balls
        //Future Sight Charge
        //aurora beam
        //aurora beam
        //Future Sight Hit (AOE)
        //pyscho cut
        //tp to other side
        switchsides();
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

    private void psychocut()
    {
        

        for(int i = 0; i <= 5; ++i)
        {
            GameObject cut_obj = Instantiate(pyschocut_prefab, firepoint.transform);
            Vector2 directionVector;
            if (facing_right)
                directionVector = Vector2.right;
            else
            {
                directionVector = Vector2.left;
                cut_obj.GetComponent<SpriteRenderer>().flipX = true;
            }
            cut_obj.GetComponent<Rigidbody2D>().AddForce(directionVector * pyscho_cut_force);
            cut_obj.transform.rotation = new Quaternion(0, 0, 36 * i, 0);
        }
    }

    private void aurorabeam()
    {

    }

    private void futuresight()
    {

    }

    private void doubleteam()
    {

    }

    private void moonbeam() //heal
    {

    }

    private void moonblast() //dmg
    {

    }

}
