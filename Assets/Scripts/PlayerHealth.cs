using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health;
    public int max_health;
    private  static GameObject my_health_bar;
    public GameObject death_ui;

    private static GameObject bosshp_bar;

    private GameObject bosstrigger;
    private GameObject curboss;

    public AudioSource take_damage_audio;

    private bool immune = false;

    private void Start()
    {
        my_health_bar = GameObject.FindGameObjectWithTag("PlayerHPUI'");
        bosshp_bar = GameObject.FindGameObjectWithTag("BOSSHPUI");
    }

    public void heal_player(int heal)
    {
        player_health = Mathf.Min(player_health + heal, max_health);
        my_health_bar.GetComponent<HealthBar>().SetHealth(player_health);
        Debug.Log(max_health);
        Debug.Log(player_health);
    }
    public void takeDamage(int dmg)
    {
        if (immune == false)
        {
            take_damage_audio.Play();
            player_health -= dmg;
            my_health_bar.GetComponent<HealthBar>().SetHealth(player_health);
            StartCoroutine(dmgImmune());
            if (player_health <= 0)
            {
                GameObject[] ais = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject ai in ais)
                {
                    Debug.Log(ai.layer);
                    if (ai.layer == LayerMask.NameToLayer("simple_ai"))
                    {
                        Destroy(ai);
                    }
                }
                Debug.Log("Ded");
                death_ui.GetComponent<DeathUIDoc>().enabledeathroot();
                //die to boss
                if (curboss != null)
                    curboss.SetActive(false);
                if (bosstrigger != null)
                {
                    Debug.Log("reactivate trigger");
                    bosstrigger.SetActive(true);
                }
                bosshp_bar.GetComponent<BossHealthBar>().disableroot();
                StopAllCoroutines();
                //turn on boss trigger
                immune = false;
                gameObject.SetActive(false);
                //play death stuff
            }
        }

    }

    IEnumerator dmgImmune()
    {
        immune = true;
        yield return new WaitForSeconds(.75f);
        immune = false;
    }

    public void SetBossTrigger(GameObject trigger)
    {
        this.bosstrigger = trigger;
        Debug.Log("Set boss Trigger: " + bosstrigger.name);
    }

    public void removeBossTrigger()
    {
        bosstrigger = null;
        curboss = null;
        Debug.Log("removed boss trigger");
    }

    public void setcurboss(GameObject curboss)
    {
        this.curboss = curboss;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyWaterBeam")
        {
            int damage = collision.gameObject.GetComponent<BulletStats>().damage;
            takeDamage(damage);
        }
    }
}
