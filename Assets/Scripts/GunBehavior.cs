using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public List<GameObject> guns;
    public GameObject specialGun;

    GameObject gunObject;
    GunStats gunStats;
    PlayerMoveTest script;
    GameObject player;

    float cooldown_timestamp;
    int special_gun_counter = 0;
    int last_gun_index = 0;
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

        player = GameObject.FindGameObjectsWithTag("Player")[0];
        script = player.GetComponent<PlayerMoveTest>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldown_timestamp <= Time.time && gunObject.name != "hemoroaid(Clone)")
        {
            cooldown_timestamp = Time.time + gunStats.cooldown;
            gunObject.SendMessage("Shoot");
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            SwapGun();
        }
    }

    void SwapGun()
    {
        Destroy(gunObject);
        GameObject new_gun;
        if (special_gun_counter >= 10 && Random.Range(0, 4) == 1)
        {
            special_gun_counter = 0;
            new_gun = specialGun;
        }
        else
        {
            int new_gun_index = Random.Range(0, guns.Count);
            while (last_gun_index == new_gun_index)
            {
                new_gun_index = Random.Range(0, guns.Count);
            }
            new_gun = guns[new_gun_index];
            last_gun_index = new_gun_index;
            special_gun_counter++;
        }
        gunObject = Instantiate(new_gun, transform.position, Quaternion.identity);
        gunObject.transform.parent = gameObject.transform.parent.transform;
        gunStats = gunObject.GetComponent<GunStats>();

        Debug.Log(gunObject.name);
        if(gunObject.name == "cumbee(Clone)")
        {
            gunObject.transform.eulerAngles = new Vector3(180, 180, 0);
        }
        else
        {
            gunObject.transform.eulerAngles = new Vector3(180, 0, 0);
        }
        if (!script.looking_right)
        {
            gunObject.transform.localScale = new Vector3(-gunObject.transform.localScale.x, gunObject.transform.localScale.y, gunObject.transform.localScale.z);
        }
    }
}
