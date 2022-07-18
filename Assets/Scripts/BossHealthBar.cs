using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BossHealthBar : MonoBehaviour
{
    public GameObject hp_bar_ui;
    private VisualElement hp_fill;
    private int width = 1300;

    private VisualElement boss_root;
    
    void Start()
    {
        boss_root = GetComponent<UIDocument>().rootVisualElement;

        hp_fill = boss_root.Q<VisualElement>("Fill2");

        disableroot();
    }

    public void SetHealth(int health, int max_health)
    {
        Debug.Log("healh: " + health +"max_health: " + max_health);
        hp_fill.style.width = (width / max_health) * health;
    }

    public void enableroot()
    {
        boss_root.style.display = DisplayStyle.Flex;
    }

    public void disableroot()
    {
        boss_root.style.display = DisplayStyle.None;
    }

}
