using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayer : MonoBehaviour
{
    public bool vrScene = true;
    public string vrDevice = "Oculus";
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("VRSwitch");
    }
    private IEnumerator VRSwitch ()
    {
        if (vrScene == false)
        {
            yield return new WaitForEndOfFrame();
            UnityEngine.XR.XRSettings.LoadDeviceByName("None");
            yield return new WaitForEndOfFrame();
            UnityEngine.XR.XRSettings.enabled = false;
        }
        else
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("XR Device: " + UnityEngine.XR.XRSettings.loadedDeviceName);
            if (UnityEngine.XR.XRSettings.loadedDeviceName != vrDevice)
            {
                UnityEngine.XR.XRSettings.LoadDeviceByName(vrDevice);
            }
            yield return new WaitForEndOfFrame();
            UnityEngine.XR.XRSettings.enabled = true;
        }
    }

}
