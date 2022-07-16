using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health = 10;
    public int max_health;

    private bool immune = false;


    public void heal_player(int heal)
    {
        player_health = Mathf.Min(player_health + heal, max_health);
        Debug.Log("healed");
    }
    public void takeDamage(int dmg)
    {
        if (immune == false)
        {
            player_health -= dmg;
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
