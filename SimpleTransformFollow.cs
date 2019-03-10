using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTransformFollow : MonoBehaviour
{
    public GameObject target;   // target to follow
    public Vector3 offset;      // offset

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position - offset; 
    }
}
