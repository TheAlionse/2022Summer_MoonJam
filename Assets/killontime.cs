using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killontime : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("killplayer");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StopCoroutine("killplayer");
    }

    IEnumerator killplayer()
    {
        yield return new WaitForSeconds(7f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().takeDamage(1000);
    }
}
