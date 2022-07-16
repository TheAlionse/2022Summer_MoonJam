using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadSpawner : MonoBehaviour
{
    public float spawn_timer;
    public GameObject borpmon_prefab;

    public Collider2D my_collider;

    private bool waiting;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!waiting)
        {
            StartCoroutine("Spawner");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine("Spawner");
        waiting = false;
    }

    IEnumerator Spawner()
    {
        //maybe check if not in camera later
        Vector3 spawn_point = new Vector3(Random.Range(my_collider.bounds.min.x, my_collider.bounds.max.x), Random.Range(my_collider.bounds.min.y, my_collider.bounds.max.y),  0);
        GameObject spawned = Instantiate(borpmon_prefab);
        spawned.transform.position = spawn_point;
        waiting = true;
        yield return new WaitForSeconds(spawn_timer);
        waiting = false;
        StartCoroutine("Spawner");
    }
}
