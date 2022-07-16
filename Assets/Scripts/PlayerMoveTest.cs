using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;

    private Vector3 my_input;

    // Update is called once per frame
    void Update()
    {
        my_input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + my_input * Time.deltaTime * speed);
    }
}
