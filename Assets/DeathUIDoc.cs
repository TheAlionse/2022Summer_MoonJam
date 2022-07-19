using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathUIDoc : MonoBehaviour
{
    public Button retry_button;
    public GameObject my_player;
    public Inventory my_inventory;

    private VisualElement my_root;
    private bool amdead = false;

    // Start is called before the first frame update
    void Start()
    {
        my_root = GetComponent<UIDocument>().rootVisualElement;
        retry_button = my_root.Q<Button>("RetryButton");
        disabledeathroot();
    }

    void RetryButtonPressed()
    {
        Debug.Log("clicked");
        my_player.GetComponent<PlayerHealth>().heal_player(10000);
        my_player.SetActive(true);
        //tp to respawn point
  
        my_player.transform.position = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMoveTest>().respawn_point;
        //reset boss if on it
        if (my_player.GetComponentInChildren<GunBehavior>().guns.Count >= 2)
        {
            my_inventory.TurnOnWeaponTimer();
            StartCoroutine(my_player.GetComponentInChildren<GunBehavior>().RandomizeGun());
        }

        disabledeathroot();
        Debug.Log("made it through");
    }

    public void enabledeathroot()
    {
        amdead = true;
        my_root.style.display = DisplayStyle.Flex;
        retry_button.clicked += RetryButtonPressed;
    }

    public void disabledeathroot()
    {
        amdead = false;
        my_root.style.display = DisplayStyle.None;
    }
}
