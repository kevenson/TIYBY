// AntSpawner.cs
// Ant spawner script
// Kai Evenson

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntSpawner : MonoBehaviour
{
    // set start position for prefabs
    public Transform startPosition;
    // set prefab or prefab variant
    public GameObject prefabToInstantiate;
    // custom delay between spawns
    public int spawnDelay = 1;
    
    [Header("Number of Prefabs to instantiate")]
    public int instNumber;


    void Start()
    {
        // Make sure we have the right assignments and then begin spawning coroutine
        if (startPosition == null || prefabToInstantiate == null)
        {
            return;
        }
        else {
            StartCoroutine("SpawnAnt");
        }
    }

    // Coroutine for spawning ants after delay
    IEnumerator SpawnAnt()
    {
        for (int i = 0; i < instNumber; i++)
        {
            // instantiate selected num of ant prefabs
            GameObject newAnt = Instantiate(prefabToInstantiate, startPosition.position, Quaternion.identity);
            //Make sure objects are active
            newAnt.SetActive(true);
            //Delay before spawning new ant
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}
