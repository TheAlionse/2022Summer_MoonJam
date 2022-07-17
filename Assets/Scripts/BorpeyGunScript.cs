using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorpeyGunScript : MonoBehaviour
{
    public GameObject bulletSpawn;
    public GameObject slash;
    public GunStats gunStats;

    PlayerMoveTest script;
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        script = player.GetComponent<PlayerMoveTest>();
    }

    public void Shoot()
    {
        StartCoroutine(ShootFunction());
    }

    IEnumerator ShootFunction()
    {
        GameObject slash_instance = Instantiate(slash, bulletSpawn.transform.position, Quaternion.identity);
        slash_instance.GetComponent<BulletStats>().damage = gunStats.damage;
        slash_instance.transform.parent = gameObject.transform;
        if (!script.looking_right)
        {
            slash_instance.transform.eulerAngles = new Vector3(0,180,0);
        }
        yield return new WaitForSeconds(0.2f);
        Destroy(slash_instance);
    }
}
