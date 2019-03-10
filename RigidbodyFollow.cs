using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyFollow : MonoBehaviour
{
    public Transform handTarget;
    private Rigidbody rb;
    public bool isLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Using FixdUpdate for Rigidbody movment
    void FixedUpdate()
    {
        rb.MovePosition(handTarget.position);
        //if (isLeft)
        //{
        //    rb.MovePosition(leftHandTarget.position);
        //}
        //else
        //{
        //    rb.MovePosition(rightHandTarget.position);
        //}
    }
}
