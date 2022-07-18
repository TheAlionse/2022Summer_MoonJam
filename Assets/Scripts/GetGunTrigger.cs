using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGunTrigger : MonoBehaviour
{
    public GameObject route_block;
    public GameObject player_gun;
    public GameObject gun_prefab;
    public GameObject spawn_something;
    public bool is_legend;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //give player gun
        if(gun_prefab != null)
        {
            if(is_legend)
                player_gun.GetComponent<GunBehavior>().AddSpecialGun(gun_prefab);
            else
            player_gun.GetComponent<GunBehavior>().AddGun(gun_prefab);
        }
        //disable any route blocks
        if(route_block != null)
        {
            route_block.SetActive(false);
        }
        if(spawn_something != null)
        {
            spawn_something.SetActive(true);
        }
        gameObject.SetActive(false);
    }
}
