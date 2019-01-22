using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdFlight : MonoBehaviour
{
    // NOTE: think I'll want to update to BOIDS algorithm: http://wiki.unity3d.com/index.php/Flocking#C.23_edition

    public float horizontalSpeed = 1f;
    public float verticalSpeed = 1f;
    public float amplitude = 1f;

    public Vector3 tempPosition;


    // Start is called before the first frame update
    void Start()
    {
        tempPosition = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        tempPosition.z += horizontalSpeed;
        tempPosition.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSpeed) * amplitude;
        transform.position = tempPosition;
    }
}
