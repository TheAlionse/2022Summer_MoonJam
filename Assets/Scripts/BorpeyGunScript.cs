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
        Vector3 look_dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - bulletSpawn.transform.position;
        Vector3 new_position = bulletSpawn.transform.position + gunStats.range_or_duration * look_dir.normalized;
        GameObject slash_instance = Instantiate(slash, new_position, Quaternion.identity);
        slash_instance.GetComponent<BulletStats>().damage = gunStats.damage;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;

        slash_instance.transform.parent = gameObject.transform;
        if (!script.looking_right)
        {
            slash_instance.transform.eulerAngles = new Vector3(0,180,0);
        }
        slash_instance.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        yield return new WaitForSeconds(0.2f);
        Destroy(slash_instance);


    }
}
