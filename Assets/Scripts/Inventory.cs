using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    public enum Weapon { CumBee, Kadorpra, Hemoroaid, Coomelia, Borpey, Coomdra };

    private Button debug_button;
    private VisualElement timer_fill;
    private VisualElement timer_fill_bg;
    private float width = 125;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        debug_button = root.Q<Button>("DebugButton");
        timer_fill = root.Q<VisualElement>("TimerFill");
        timer_fill_bg = root.Q<VisualElement>("TimerFillBG");
        ClearInventorySlot(1);
        TurnOffWeaponTimer();

        // How to use Weapon Countdown:
        // Inventory.TurnOnWeaponTimer();
        // StartCoroutine(Inventory.StartWeaponRotateCountdown(10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOffWeaponTimer()
    {
        timer_fill.style.display = DisplayStyle.None;
        timer_fill_bg.style.display = DisplayStyle.None;
    }

    public void TurnOnWeaponTimer()
    {
        timer_fill.style.display = DisplayStyle.Flex;
        timer_fill_bg.style.display = DisplayStyle.Flex;
    }

    public IEnumerator StartWeaponRotateCountdown(int timeInSeconds)
    {
        for (int i = 1; i <= timeInSeconds * 2; i++)
        {
            timer_fill.style.width = width - (width / (timeInSeconds * 2) * i);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void ClearInventorySlot(int slot)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        var item_box = root.Q<VisualElement>("ItemBox" + slot);
        foreach (Weapon weapon in Enum.GetValues(typeof(Weapon)))
        {
            root.Q<VisualElement>(weapon.ToString() + slot).style.display = DisplayStyle.None;
        }
    }

    public void SetInventorySlotToWeapon(int slot, Weapon weapon)
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        ClearInventorySlot(slot);
        root.Q<VisualElement>(weapon.ToString() + slot).style.display = DisplayStyle.Flex;
    }
}
