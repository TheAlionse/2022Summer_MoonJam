using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadSpawner : MonoBehaviour
{
    public float spawn_timer;
    public GameObject[] borpmon_prefabs;
    public GameObject SpawnerManager;

    public CircleCollider2D my_collider;

    private bool waiting;
    SpawnerManager sm;

    private void Start()
    {
        sm = SpawnerManager.GetComponent<SpawnerManager>();
    }
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
        if (!sm.disableSpawning)
        {
            float x = ((my_collider.radius / 2) + 1) * 2;
            float random_x = Random.Range(my_collider.radius / 2, (my_collider.radius / 2) + x);
            if (random_x > my_collider.radius)
            {
                random_x = (x / 2) - random_x;
            }

            float random_y = Random.Range(my_collider.radius / 2, (my_collider.radius / 2) + x);
            if (random_y > my_collider.radius)
            {
                random_y = (x / 2) - random_y;
            }

            Vector3 spawn_point = new Vector3(random_x + transform.position.x, random_y + transform.position.y, 0);
            GameObject spawned = Instantiate(borpmon_prefabs[Random.Range(0, borpmon_prefabs.Length)]);
            spawned.transform.position = spawn_point;
            sm.Increment();
        }

        waiting = true;
        yield return new WaitForSeconds(spawn_timer);
        waiting = false;
        StartCoroutine("Spawner");
    }
}
