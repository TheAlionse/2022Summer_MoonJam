using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetGunTrigger : MonoBehaviour
{
    public GameObject route_block;
    public GameObject give_gun;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //give player gun
        if(give_gun != null)
        {
            //give player gun
        }
        //disable any route blocks
        if(route_block != null)
        {
            route_block.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
