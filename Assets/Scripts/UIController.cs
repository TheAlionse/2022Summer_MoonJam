using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    public GameObject menu_ui;
    public PauseScript pause_script;
    public Button resume_button;
    public Button exit_button;

    // Start is called before the first frame update
    void Start()
    {
        // No-op
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        resume_button = root.Q<Button>("ResumeButton");
        exit_button = root.Q<Button>("ExitButton");

        resume_button.clicked += ResumeButtonPressed;
        exit_button.clicked += ExitButtonPressed;
    }

    void ResumeButtonPressed() {
        Time.timeScale = 1f;
        pause_script.paused = false;
        menu_ui.SetActive(false);
    }

    void ExitButtonPressed() {
        Application.Quit();
    }
}
