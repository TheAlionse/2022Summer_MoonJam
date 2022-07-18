using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health = 10;
    public int max_health;
    private  static GameObject my_health_bar;
    public GameObject death_ui;


    public AudioSource take_damage_audio;

    private bool immune = false;

    private void Start()
    {
        my_health_bar = GameObject.FindGameObjectWithTag("PlayerHPUI'");
        Debug.Log(my_health_bar);
    }

    public void heal_player(int heal)
    {
        Debug.Log(gameObject.name);
        player_health = Mathf.Min(player_health + heal, max_health);
        my_health_bar.GetComponent<HealthBar>().SetHealth(player_health);
        Debug.Log("healed");
    }
    public void takeDamage(int dmg)
    {
        if (immune == false)
        {
            take_damage_audio.Play();
            player_health -= dmg;
            my_health_bar.GetComponent<HealthBar>().SetHealth(player_health);
            StartCoroutine(dmgImmune());
            Debug.Log(player_health);
            if (player_health <= 0)
            {
                Debug.Log("Ded");
                death_ui.SetActive(true);
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
}
