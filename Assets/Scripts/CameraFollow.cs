using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    public float smooth_time;
    private Vector3 velocity = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 target_position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, target_position, ref velocity, smooth_time);
    }
}
