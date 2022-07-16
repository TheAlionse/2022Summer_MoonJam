using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int player_health = 10;

    public void takeDamage(int dmg)
    {
        player_health -= dmg;
        Debug.Log(player_health);
        if(player_health <= 0)
        {
            Debug.Log("Ded");
            //play death stuff
        }
    }
}
