using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaling_LoadScreen : MonoBehaviour
{
    private float floorHeight;
    public static float playerScale = 1.8f;
    public static float scaler;
    [Tooltip("This value will be * by 5 for total duration as part of height calibration")]
    public float shrinkDelay = .3f; 
    public OVRPlayerController OVRController;
    //public StartCalibrationMenu startCalibrationMenu;


    public void StartCalibration()
    {
        StartCoroutine(GetHeight());

    }

    IEnumerator GetHeight()
    {
        // need to sample OVRController.CameraHeight over first couple secs to get non-zero value
        // note that this only works properly if the HMD is on and user is in position when this starts
        // probably want to have an override in menu settings so this can be manually set 
        //Debug.Log("Entering Calibration Coroutine");
        //Debug.Log(OVRController.CameraHeight);
        while (OVRController.CameraHeight < 0.1f)
        {
            yield return new WaitForSeconds(.2f);
        }
        var average = 0f;
        for (int i =0; i < 5; i++)
        {
            average += OVRController.CameraHeight;
            yield return new WaitForSeconds(shrinkDelay);
        }
        floorHeight = average / 5;
        scaler = playerScale / floorHeight;
        StartCalibrationMenu.calibrationComplete = true;
        yield return scaler;
    }


}
