using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScaling : MonoBehaviour
{
    //public Vector3 scaler;
    public int scaler = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition *= scaler;
    }
}
