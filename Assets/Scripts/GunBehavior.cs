using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    public List<GameObject> guns;
    public GameObject specialGun;
    public float gunSwapCooldown;
    public float specialGunSwapCooldown;

    GameObject gunObject;
    GunStats gunStats;
    PlayerMoveTest script;
    GameObject player;

    float cooldown_timestamp;
    int special_gun_counter = 0;
    int last_gun_index = 0;
    Coroutine coroutine;

    public Inventory my_inventory;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] guns = GameObject.FindGameObjectsWithTag("Gun");

        foreach (GameObject gun in guns)
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
        if (Input.GetMouseButtonDown(0) && cooldown_timestamp <= Time.time && gunObject.name != "hemoroaid(Clone)" && gunObject!= null)
        {
            cooldown_timestamp = Time.time + gunStats.cooldown;
            gunObject.SendMessage("Shoot");
        }
    }

    void SwapGun()
    {
        if(guns.Count >= 2)
        {
            Destroy(gunObject);
            GameObject new_gun;

            if (special_gun_counter >= 10 && Random.Range(0, 4) == 1)
            {
                last_gun_index = -1;
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
                if(specialGun != null)
                {
                    special_gun_counter++;
                }
            }
            EquipGun(new_gun);
        }
        
    }

    public void AddGun(GameObject gun_prefab)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        guns.Add(gun_prefab);
        if(gunObject != null)
        {
            Destroy(gunObject);
        }
        last_gun_index = guns.Count - 1;
        EquipGun(gun_prefab);

        if(guns.Count >= 2)
        {
            my_inventory.TurnOnWeaponTimer();
            coroutine = StartCoroutine(RandomizeGun());
        }
    }

    public void AddSpecialGun(GameObject gun_prefab)
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        specialGun = gun_prefab;
        if (gunObject != null)
        {
            Destroy(gunObject);
        }
        last_gun_index = -1;
        EquipGun(gun_prefab);
        if (guns.Count >= 2)
        {
            coroutine = StartCoroutine(RandomizeGun());
        }
    }

    void EquipGun(GameObject new_gun)
    {
        string mygun = getcurgun();
        //CumBee, Kadorpra, Hemoroaid, Coomelia, Borpey, Coomdra
        if (mygun == "borpey")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.Borpey);
        }
        else if(mygun == "cumbee")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.CumBee);
        }
        else if (mygun == "Coomelia")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.Coomelia);
        }
        else if (mygun == "hemorald")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.Hemoroaid);
        }
        else if (mygun == "coomdra")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.Coomdra);
        }
        else if (mygun == "kadorpra")
        {
            my_inventory.SetInventorySlotToWeapon(1, Inventory.Weapon.Kadorpra);
        }

        gunObject = Instantiate(new_gun, transform.position, Quaternion.identity);
        gunObject.transform.parent = gameObject.transform.parent.transform;
        gunStats = gunObject.GetComponent<GunStats>();

        if (gunObject.name == "cumbee(Clone)")
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
        cooldown_timestamp = Time.time;

    }

    IEnumerator RandomizeGun()
    {
        while (true)
        {
            if (last_gun_index == -1)
            {
                StartCoroutine(my_inventory.StartWeaponRotateCountdown((int)specialGunSwapCooldown));
                yield return new WaitForSeconds(specialGunSwapCooldown);
            }
            else
            {
                StartCoroutine(my_inventory.StartWeaponRotateCountdown((int)gunSwapCooldown));
                yield return new WaitForSeconds(gunSwapCooldown);
            }

            SwapGun();
        }
    }

    public string getcurgun()
    {
        GameObject mygun = guns[last_gun_index];
        return guns[last_gun_index].name;
    }
}
