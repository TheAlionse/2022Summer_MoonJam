using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillAllAITrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameObject[] ais = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject ai in ais)
            {
                Debug.Log(ai.layer);
                if(ai.layer == LayerMask.NameToLayer("simple_ai"))
                {
                    Destroy(ai);
                }
            }
        }
    }
}
