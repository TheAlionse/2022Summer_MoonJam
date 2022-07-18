using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerManager : MonoBehaviour
{
    public int maxUnitCount;
    [HideInInspector] public int currentUnitCount;
    public bool disableSpawning = false;

    public void Increment()
    {
        currentUnitCount++;
        if(currentUnitCount >= maxUnitCount)
        {
            disableSpawning = true;
        }
    }

    public void Decrement()
    {
        currentUnitCount--;
        disableSpawning = false;
    }

    public void Reset()
    {
        disableSpawning = false;
        currentUnitCount = 0;
    }
}
