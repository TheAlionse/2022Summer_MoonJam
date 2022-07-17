using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDMG : MonoBehaviour
{
    public int my_dmg;
    public bool destory_me;

    public int give_dmg()
    {
        //figure out how it should be done with watergun might have to not use on trigger enter
        if (destory_me)
        {
            StartCoroutine("killme");
        }
        return my_dmg;
    }

    IEnumerator killme()
    {
        yield return new WaitForSeconds(.05f);
        Destroy(gameObject);
    }
}
