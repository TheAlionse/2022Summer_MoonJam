using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHealthBar : MonoBehaviour
{
    public GameObject hp_bar_ui;
    private VisualElement hp_fill;
    private int width = 1300;

    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        hp_fill = root.Q<VisualElement>("Fill");
    }

    public void SetHealth(int health, int max_health)
    {
        hp_fill.style.width = (width / max_health) * health;
    }

}
