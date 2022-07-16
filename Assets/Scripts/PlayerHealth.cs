using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health = 10;

    private bool immune = false;

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
