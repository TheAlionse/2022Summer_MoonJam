using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CumbeeBulletScript : MonoBehaviour
{
    public float oscillate_speed;
    public float oscillate_scale;
    float timer;
    public float speed;
    public float angleChangingSpeed;
    public float max_homing_latch_angle;


    public float pull_strength = 1f;

    bool ignore_homing = false;
    GameObject target;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        target = FindClosestEnemyToMouse();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        timer += Time.deltaTime;
        Vector2 direction = (Vector2)target.transform.position - rb.position;
        gameObject.transform.Translate(0, oscillate(timer, oscillate_speed, oscillate_scale),0);

        if(!ignore_homing)
        {
            if (Mathf.Abs(Vector2.Angle(direction, rb.velocity)) < max_homing_latch_angle)
            {
                if (!V2Equals(direction, rb.velocity))
                {
                    direction.Normalize();
                    Vector2 velocity = rb.velocity + (direction * pull_strength);
                    rb.velocity = velocity.normalized * speed;
                }

                float rotateAmount = Vector3.Cross(Vector2.Perpendicular(direction), transform.up).z;
                rb.angularVelocity = -angleChangingSpeed * rotateAmount;
            }
            else
            {
                ignore_homing = true;
            }
        }
    }

    float oscillate(float time, float speed, float scale)
    {
        return Mathf.Cos(time * speed / Mathf.PI) * scale;
    }

    public GameObject FindClosestEnemyToMouse()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition); foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        Debug.Log(closest.gameObject.name);
        return closest;
    }

    bool V2Equals(Vector2 lhs, Vector2 rhs)
    {
        return Vector2.SqrMagnitude(lhs - rhs) < 9.99999944E-11f;
    }

    private void OnDestroy()
    {
        GameObject.Find("cumbee").SendMessage("BeeDestroyed");
    }
}
