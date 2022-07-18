using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterLongDuration : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyObject",10f);
    }

    void DestroyObject()
    {
        Destroy(gameObject);
    }
}
