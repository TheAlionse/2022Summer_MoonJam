using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    public GameObject menu_ui;

    private bool paused;
    private void Start()
    {
        paused = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                menu_ui.SetActive(false);
                Time.timeScale = 1f;
                paused = false;
            }
            else
            {
                menu_ui.SetActive(true);
                Time.timeScale = 0f;
                paused = true;
            }
        }
    }
}
