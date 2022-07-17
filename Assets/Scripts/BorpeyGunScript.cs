using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorpeyGunScript : MonoBehaviour
{
    public GameObject bulletSpawn;
    public GameObject slash;

    bool disable_slash = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !disable_slash)
        {
            StartCoroutine(shoot());
        }
    }

    IEnumerator shoot()
    {
        disable_slash = true;
        GameObject slash_instance = Instantiate(slash, bulletSpawn.transform.position, Quaternion.identity);
        slash_instance.transform.parent = gameObject.transform;
        yield return new WaitForSeconds(0.23f);

        Destroy(slash_instance);
        disable_slash = false;
    }
}
