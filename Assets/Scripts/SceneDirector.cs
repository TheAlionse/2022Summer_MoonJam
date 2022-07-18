using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;

public class SceneDirector : MonoBehaviour
{
    public GameObject Coomelia;
    public GameObject Lightning_boss;
    public GameObject Dark_boss;

    public GameObject gun_pickup1;
    public GameObject gun_pickup2;
    public GameObject gun_pickup3;
    public GameObject gun_pickup4;
    public GameObject gun_pickup5;

    public GameObject bosshp_bar;
    public GameObject my_player;
    public GameObject fade_ui;
    private FadeScript fade_script;

    private DialogueRunner dialogue_runner;
    private void Awake()
    {
        dialogue_runner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogue_runner.AddCommandHandler("spawnlegend", spawnLegend);
        dialogue_runner.AddCommandHandler("spawnlightning", spawnlightning);
        dialogue_runner.AddCommandHandler("spawndark", spawndark);
        dialogue_runner.AddCommandHandler("stopmove", stopmove);
        dialogue_runner.AddCommandHandler("resumemove", resumemove);

        fade_script = fade_ui.GetComponent<FadeScript>();
        dialogue_runner.AddCommandHandler<float>("fadeout", fadeout);
        dialogue_runner.AddCommandHandler<string>("loadscene", loadscene);
        dialogue_runner.AddCommandHandler<GameObject>("flip", flipchar);
        dialogue_runner.AddCommandHandler("gun1", gun1);
        dialogue_runner.AddCommandHandler("gun2", gun2);
        dialogue_runner.AddCommandHandler("gun3", gun3);
        dialogue_runner.AddCommandHandler("gun4", gun4);
        dialogue_runner.AddCommandHandler("gun5", gun5);
    }


    private void spawnLegend()
    {
        my_player.GetComponent<PlayerHealth>().setcurboss(Coomelia);
        Coomelia.SetActive(true);
    }
    private void spawnlightning()
    {
        my_player.GetComponent<PlayerHealth>().setcurboss(Lightning_boss);
        Lightning_boss.SetActive(true);
    }
    private void spawndark()
    {
        my_player.GetComponent<PlayerHealth>().setcurboss(Dark_boss);
        Dark_boss.SetActive(true);
    }

    private void stopmove()
    {
        my_player.GetComponent<PlayerMoveTest>().can_move = false;
    }

    private void resumemove()
    {
        my_player.GetComponent<PlayerMoveTest>().can_move = true;
    }

    private Coroutine fadeout(float time = 1f)
    {
        return StartCoroutine(fade_script.changeAlpha(1, time));
    }

    private void loadscene(string scenename)
    {
        SceneManager.LoadScene(scenename, LoadSceneMode.Single);
    }

    private void flipchar(GameObject character)
    {
        character.GetComponent<SpriteRenderer>().flipX = !character.GetComponent<SpriteRenderer>().flipX;
    }


    //Just hard code everthing :))))))))))))
    private void gun1()
    {
        gun_pickup1.SetActive(true);
    }

    //these might not even be needed
    private void gun2()
    {
        gun_pickup2.SetActive(true);
    }
    private void gun3()
    {
        gun_pickup3.SetActive(true);
    }
    private void gun4()
    {
        gun_pickup4.SetActive(true);
    }
    private void gun5()
    {
        gun_pickup5.SetActive(true);
    }
}
