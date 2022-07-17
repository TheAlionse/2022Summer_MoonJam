using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkBossFight : MonoBehaviour
{
    public int health;
    private Renderer my_ren;

    private bool am_immune;
    private int phase = 1;
    private int phase_change1;

    private void Start()
    {
        phase_change1 = (int)(health * .5);
        am_immune = false;
        my_ren = my_ren = gameObject.GetComponent<Renderer>();
        StartCoroutine("DarkFight");
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

    IEnumerator DarkFight()
    {
        yield return new WaitForSeconds(2f);
        if (phase == 1)
        {
            //Shadow ball attack
            yield return new WaitForSeconds(2f);
        }
        else if (phase == 2)
        {
            //Run at player
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("DarkFight");
    }
}
