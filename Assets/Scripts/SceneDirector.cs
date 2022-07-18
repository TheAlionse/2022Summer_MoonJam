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
}
