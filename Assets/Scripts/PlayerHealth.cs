using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health = 10;
    public int max_health;
    public HealthBar health_bar;

    public AudioSource take_damage_audio;

    private bool immune = false;


    public void heal_player(int heal)
    {
        player_health = Mathf.Min(player_health + heal, max_health);
        health_bar.SetHealth(player_health);
        Debug.Log("healed");
    }
    public void takeDamage(int dmg)
    {
        if (immune == false)
        {
            take_damage_audio.Play();
            player_health -= dmg;
            health_bar.SetHealth(player_health);
            StartCoroutine(dmgImmune());
            Debug.Log(player_health);
            if (player_health <= 0)
            {
                Debug.Log("Ded");
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
