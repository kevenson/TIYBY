using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_billboard : MonoBehaviour
{
    // for Mods see: http://wiki.unity3d.com/index.php/CameraFacingBillboard
    static Transform tCam = null;
    void Update()
    {
        if (!tCam)
        {
            if (!Camera.main)
            {
                return;
            }
            tCam = Camera.main.transform;
        }
        transform.LookAt(tCam.position, Vector3.up);
    }
}
