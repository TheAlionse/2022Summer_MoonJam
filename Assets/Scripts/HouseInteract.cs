using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseInteract : MonoBehaviour
{
    public GameObject player;
    public bool is_heal = false;
    public bool is_gym = false;
    public GameObject obj_coord = null;
    public GameObject fade_ui = null;

    public AudioSource scream;


    private MeshRenderer my_text;
    private bool inside;
    private FadeScript fade_script;

    private void Start()
    {
        my_text = GetComponent<MeshRenderer>();
        fade_script = fade_ui.GetComponent<FadeScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        my_text.enabled = true;
        if (is_heal)
        {
            StartCoroutine("checkHeal");
        }
        else if (is_gym)
        {
            StartCoroutine("enterGym");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        my_text.enabled = false;
        if (is_heal)
        {
            StopCoroutine("checkHeal");
        }
        else if (is_gym)
        {
            StopCoroutine("enterGym");
        }
    }

    IEnumerator checkHeal() {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(scream != null)
            {
                scream.Play();
            }
            player.GetComponent<PlayerHealth>().heal_player(1000);
            player.GetComponent<PlayerMoveTest>().respawn_point = player.transform.position;
            Debug.Log(player.GetComponent<PlayerMoveTest>().respawn_point);
        }
        yield return null;
        StartCoroutine("checkHeal");
    }

    IEnumerator enterGym()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine("tp");
        }
        yield return null;
        StartCoroutine("enterGym");
    }

    IEnumerator tp()
    {
        //might want to lock playermovement
        Debug.Log("hit e");
        //fade to black
        if(scream != null)
            scream.Play();
        FadeOut();
        yield return new WaitForSeconds(1f);
        player.transform.position = obj_coord.transform.position;
        yield return new WaitForSeconds(.5f);
        FadeIn();
        //fade back
        yield return null;
    }

    private Coroutine FadeOut(float time = 1f)
    {
        return StartCoroutine(fade_script.changeAlpha(1, time));
    }
    private Coroutine FadeIn(float time = 1f)
    {
        return StartCoroutine(fade_script.changeAlpha(0, time));
    }
}
