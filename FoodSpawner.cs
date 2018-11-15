// Food Spawner.cs
// Food spawner script that spawns pile of food objects at specified location
// Kai Evenson

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // set start position for prefabs
    public Transform startPos;
    // set prefab or prefab variant
    public GameObject objectToInstantiate;
    // how far apart to spawn objects
    public float spacing = .1f;
    [Header("Number of Prefabs to instantiate")]
    public int numberOfObjects;

    void Start()
    {
        if (startPos == null || objectToInstantiate == null)
        {
            return;
        }
        else
        {
            for (int i = 0; i < numberOfObjects; i++)
            {
                Vector3 pos = new Vector3(0, i, 0) * spacing;
                Instantiate(objectToInstantiate, startPos.position += pos, Quaternion.identity);
            }
        }
    }

}
