using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemoroaidGunScript : MonoBehaviour
{
    public GameObject waterBeam;
    public GameObject bulletSpawn;
    public GunStats gunStats;


    public AudioSource shoot_audio;

    PlayerMoveTest script;
    GameObject player;
    GameObject waterBeamInstance = null;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        script = player.GetComponent<PlayerMoveTest>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waterBeamInstance == null && Input.GetMouseButtonDown(0))
        {
            shoot_audio.Play();
            ShootWaterBeam();
        }

        if (waterBeamInstance != null && Input.GetMouseButtonUp(0))
        {
            shoot_audio.Stop();
            Destroy(waterBeamInstance);
            waterBeamInstance = null;
        }
    }

    private void FixedUpdate()
    {

        Vector2 look_dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
        float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
        if (!script.looking_right)
        {
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.up);

        }
        else
        {
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.right);
        }
        //gameObject.transform.position = bulletSpawn.transform.position;

    }

    public void ShootWaterBeam()
    {
        //float positionX = bulletSpawn.transform.position.x + waterBeam.transform.localScale.x / 2f;
        //waterBeamInstance = Instantiate(waterBeam, new Vector2(positionX, bulletSpawn.transform.position.y), Quaternion.identity);
        waterBeamInstance = Instantiate(waterBeam, bulletSpawn.transform.position, gameObject.transform.rotation);
        waterBeamInstance.transform.parent = gameObject.transform;
        waterBeamInstance.GetComponent<BulletStats>().damage = gunStats.damage;
        waterBeamInstance.GetComponent<WaterBeamScript>().max_length = gunStats.range_or_duration;
        waterBeamInstance.GetComponent<WaterBeamScript>().speed = gunStats.velocity;
        if (!script.looking_right)
        {
            waterBeamInstance.transform.localScale = new Vector3(-waterBeamInstance.transform.localScale.x, waterBeamInstance.transform.localScale.y, waterBeamInstance.transform.localScale.z);
        }
    }
}
