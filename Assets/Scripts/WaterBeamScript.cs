using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBeamScript : MonoBehaviour
{

    public float max_length = 40f;
    public float speed = 200f;
    public float acceleration = 500f;

    float initial_speed;

    // Start is called before the first frame update
    void Start()
    {
        initial_speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.localScale.x < max_length)
        {
            Debug.Log("increasing");
            Vector3 rescale = gameObject.transform.localScale;
            rescale.x += speed * Time.deltaTime;
            gameObject.transform.localScale = rescale;
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
        Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == LayerMask.NameToLayer("enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
        {
            var collisionPoint = collision.ClosestPoint(transform.position);
            var distance = Vector2.Distance(collisionPoint, transform.position);

            float size = gameObject.GetComponent<Renderer>().bounds.size.x;
            Vector3 rescale = gameObject.transform.localScale;

            rescale.x = distance * rescale.x / size;
            if (rescale.x > max_length)
                rescale.x = max_length;
            gameObject.transform.localScale = rescale;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.layer == LayerMask.NameToLayer("enemy") || collision.gameObject.layer == LayerMask.NameToLayer("Walls"))
    //    {
    //        var collisionPoint = collision.ClosestPoint(transform.position);
    //        var distance = Vector2.Distance(collisionPoint, transform.position);

    //        float size = gameObject.GetComponent<Renderer>().bounds.size.x;
    //        Vector3 rescale = gameObject.transform.localScale;

    //        rescale.x = distance * rescale.x / size;
    //        if (rescale.x > default_length.x)
    //            rescale.x = default_length.x;
    //        gameObject.transform.localScale = rescale;
    //    }
    //}
}
