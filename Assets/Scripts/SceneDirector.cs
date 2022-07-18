using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SceneDirector : MonoBehaviour
{
    public GameObject Coomelia;
    public GameObject Lightning_boss;
    public GameObject Dark_boss;

    private DialogueRunner dialogue_runner;
    private void Awake()
    {
        dialogue_runner = FindObjectOfType<Yarn.Unity.DialogueRunner>();
        dialogue_runner.AddCommandHandler("spawnlegend", spawnLegend);
        dialogue_runner.AddCommandHandler("spawnlightning", spawnlightning);
        dialogue_runner.AddCommandHandler("spawndark", spawndark);
    }

    private void spawnLegend()
    {
        Coomelia.SetActive(true);
    }
    private void spawnlightning()
    {
        Lightning_boss.SetActive(true);
    }
    private void spawndark()
    {
        Dark_boss.SetActive(true);
    }
}
