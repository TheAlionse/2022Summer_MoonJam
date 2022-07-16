using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadMovement : MonoBehaviour
{
    public float speed;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }
}
