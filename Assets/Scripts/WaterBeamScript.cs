using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeamScript : MonoBehaviour
{

    public float max_length;
    public float speed;
    public float acceleration;
    public SpriteRenderer renderer;

    float initial_speed;

    // Start is called before the first frame update
    void Start()
    {
        initial_speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (renderer.size.x < max_length)
        {
            float rescale = renderer.size.x;
            rescale += speed * Time.deltaTime;
            renderer.size = new Vector2(rescale, 0.625f);
        }

        if (Input.GetMouseButtonDown(0))
        {
            speed += acceleration * Time.deltaTime;
        }

        if (Input.GetMouseButtonUp(0))
        {
            speed = initial_speed;
        }


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            var collisionPoint = collision.ClosestPoint(transform.position);
            var distance = Vector2.Distance(collisionPoint, transform.position);

            if(distance > max_length)
            {
                distance = max_length;
            }
            renderer.size = new Vector2(distance, 0.625f);
        }
    }
}
