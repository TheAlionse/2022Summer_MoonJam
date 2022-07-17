using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{

    GameObject gunObject;
    GunStats gunStats;

    float cooldown_timestamp;
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");

        foreach(GameObject gun in guns)
        {
            if (gun.active)
            {
                gunObject = gun;
                gunStats = gun.GetComponent<GunStats>();
                Debug.Log(gunStats.damage);
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldown_timestamp <= Time.time)
        {
            cooldown_timestamp = Time.time + gunStats.cooldown;
            gunObject.SendMessage("Shoot");
        }
    }
}
