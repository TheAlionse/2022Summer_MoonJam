using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTest : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed;
    public KeyCode dash_key;
    public float dash_speed;
    public float dash_cd_reset;
    public float dash_time;
    public Texture2D crosshair;
    public Camera cam;
    public Vector2 mouse_pos;
    public AudioSource dash_audio;
    public bool looking_right = true;

    public Vector2 respawn_point;

    public BoostBarFill boost_bar;

    private Vector3 my_input;
    private float old_speed;
    private float dash_cool_down;
    private float cur_dash_time;
    private Vector3 initialScale;

    private void Start()
    {
        old_speed = speed;
        Vector2 cursor_offset = new Vector2(crosshair.width / 2, crosshair.height / 2);
        Cursor.SetCursor(crosshair, cursor_offset, CursorMode.Auto);
        initialScale = transform.localScale;
        boost_bar.HideBoostCooldownTimer();
    }

    // Update is called once per frame
    void Update()
    {
        my_input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetKeyDown(dash_key) & dash_cool_down <= 0)
        {
            dash_audio.Play();
            boost_bar.ShowBoostCooldownTimer();
            StartCoroutine(boost_bar.StartBoostCooldownTimer(dash_cd_reset));
            this.speed = this.speed * dash_speed;
            dash_cool_down = dash_cd_reset;
            cur_dash_time = 0;
        }

        if(cur_dash_time <= dash_time)
        {
            cur_dash_time += Time.deltaTime;
        }
        else
        {
            this.speed = this.old_speed;
        }

        if (dash_cool_down >= 0f)
        {
            dash_cool_down -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + my_input * Time.deltaTime * speed);

        //might not want player to look at cursor?
        var delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        if (delta.x >= 0 && !looking_right)
        {
            transform.localScale = initialScale;
            looking_right = true;
        }
        else if(delta.x < 0 && looking_right)
        {
            transform.localScale = new Vector3(-initialScale.x, initialScale.y, initialScale.z);
            looking_right = false;
        }
    }

}
