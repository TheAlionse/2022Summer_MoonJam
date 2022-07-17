using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RodMovement : MonoBehaviour
{
    public float speed;
    public GameObject my_static;
    public float max_static_size;
    public float smoothTime;
    public float max_life;

    private bool canMove = false;
    private bool play_static = false;
    private Vector2 move_obj;
    private float currentVelocity = 0.0f;
    private float cur_scale_target;
    private bool bigger = true;

    private float time_alive;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (canMove)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, move_obj, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, move_obj) < 0.001f)
            {
                canMove = false;
                play_static = true;
                my_static.SetActive(true);
                cur_scale_target = max_static_size;
            }
        }
        else if (play_static)
        {
            time_alive += Time.deltaTime;
            float cur_scale = Mathf.SmoothDamp(my_static.transform.localScale.x, cur_scale_target, ref currentVelocity, smoothTime);
            my_static.transform.localScale = new Vector3(cur_scale, cur_scale, 1);
            if(Mathf.Abs(cur_scale_target - cur_scale) < .1f)
            {
                if (bigger)
                {
                    cur_scale_target = 0f;
                }
                else
                    cur_scale_target = max_static_size;
                bigger = !bigger;
            }

            if(time_alive >= max_life)
            {
                Destroy(gameObject);
            }
        }
    }

    public void giveCoord(float x, float y)
    {
        move_obj = new Vector2(x, y);
        canMove = true;
    }
}
