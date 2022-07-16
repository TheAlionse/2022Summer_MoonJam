using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HemoroaidGunScript : MonoBehaviour
{
    public GameObject waterBeam;
    public GameObject bulletSpawn;

    GameObject waterBeamInstance = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waterBeamInstance == null && Input.GetMouseButtonDown(0))
        {
            shoot();
        }

        if (waterBeamInstance != null && Input.GetMouseButtonUp(0))
        {
            Destroy(waterBeamInstance);
            waterBeamInstance = null;
        }
    }

    private void FixedUpdate()
    {

            Vector2 look_dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position;
            float angle = Mathf.Atan2(look_dir.y, look_dir.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward) * Quaternion.AngleAxis(180, Vector3.right);
            //gameObject.transform.position = bulletSpawn.transform.position;


    }

    public void shoot()
    {
        //float positionX = bulletSpawn.transform.position.x + waterBeam.transform.localScale.x / 2f;
        //waterBeamInstance = Instantiate(waterBeam, new Vector2(positionX, bulletSpawn.transform.position.y), Quaternion.identity);
        waterBeamInstance = Instantiate(waterBeam, bulletSpawn.transform.position, gameObject.transform.rotation);
        waterBeamInstance.transform.parent = gameObject.transform;
    }
}
