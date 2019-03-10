using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestrictChildRotation : MonoBehaviour
{
    // set the child transform.rotation to a fixed value in 
    // LateUpdate to eliminate any parent-originated rotation.
    private Quaternion initialRotation;
    // Start is called before the first frame update
    void Start()
    {
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
