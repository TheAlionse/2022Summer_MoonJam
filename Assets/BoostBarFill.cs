using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostBarFill : MonoBehaviour
{

    public GameObject fill_bar;
    public GameObject fill_bar_bg;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void HideBoostCooldownTimer()
    {
        fill_bar.GetComponent<SpriteRenderer>().enabled = false;
        fill_bar_bg.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void ShowBoostCooldownTimer()
    {
        fill_bar.GetComponent<SpriteRenderer>().enabled = true;
        fill_bar_bg.GetComponent<SpriteRenderer>().enabled = true;
    }

    public IEnumerator StartBoostCooldownTimer(float timeInSeconds) {
        for (int i = 1; i <= timeInSeconds * 2; i++)
        {
            fill_bar.transform.localScale = new Vector2((i / (timeInSeconds * 2)), 1f);
            // fill_bar.GetComponent<SpriteRenderer>().size = new Vector2((i / (timeInSeconds * 2)), 1f);
            yield return new WaitForSeconds(0.5f);
        }
        HideBoostCooldownTimer();
    }
}
