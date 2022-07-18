using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathUIDoc : MonoBehaviour
{
    public Button retry_button;
    public GameObject my_player;

    // Start is called before the first frame update
    void Start()
    {
        // No-op
    }

    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        retry_button = root.Q<Button>("RetryButton");

        retry_button.clicked += RetryButtonPressed;
    }

    void RetryButtonPressed()
    {
        my_player.SetActive(true);
        //tp to respawn point
        my_player.transform.position = my_player.GetComponent<PlayerMoveTest>().respawn_point;
        //reset boss if on it
        gameObject.SetActive(false);
    }
}
