using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeScreenshotCapture : MonoBehaviour
{
    // simple screenshot script. Should be triggered by left hand trigger
    private static int counter = 0;
    private string fileName = "HW_";
    public int imageScale = 10;

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.4f)
        {
            var day = System.DateTime.Now.ToString("MM-dd");
            var imageName = fileName + day +"_#"+ counter + ".png";
            ScreenCapture.CaptureScreenshot(imageName, imageScale);
            counter += 1;
        }
    }
}
