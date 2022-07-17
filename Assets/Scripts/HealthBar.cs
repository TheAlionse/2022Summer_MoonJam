using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthBar : MonoBehaviour
{
    public GameObject hp_bar_ui;
    private VisualElement hp_fill;
    private int max_health;
    private int width;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        max_health = 10;
        width = 330;

        hp_fill = root.Q<VisualElement>("Fill");
    }

    public void SetHealth(int health)
    {
        //replaced Max_health with 10 cause of weird error
        var new_width = (width / 10) * health;
        hp_fill.style.width = new_width;
    }

}
