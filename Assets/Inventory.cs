using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Inventory : MonoBehaviour
{

    public enum Weapon { CumBee, Kadorpra, Hemoroaid, Coomelia };

    private Button debug_button;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        debug_button = root.Q<Button>("DebugButton");
        ClearInventorySlot(1);
    }

    // Update is called once per frame
    void Update()
    {
        
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
