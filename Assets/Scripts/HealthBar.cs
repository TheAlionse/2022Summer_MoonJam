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

        Debug.Log(hp_fill.style);
    }

    public void SetHealth(int health)
    {
        var new_width = (width / max_health) * health;
        hp_fill.style.width = new_width;
    }

}