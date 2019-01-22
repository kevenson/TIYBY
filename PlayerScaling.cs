using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaling : MonoBehaviour
{
    private float floorHeight;
    public float playerScale = 1.8f;
    public OVRPlayerController OVRController;
    public Transform AntHeadParent;
    public Transform AntHead;

    // Start is called before the first frame update
    void Start()
    {

        if (OVRController == null)
        {
            return;
        }
        else
        {

            StartCoroutine(GetHeight());
        }
        
    }

    IEnumerator GetHeight()
    {
        // need to sample OVRController.CameraHeight over first couple secs to get non-zero value
        // note that this only works properly if the HMD is on and user is in position when this starts
        // probably want to have an override in menu settings so this can be manually set 

        while (OVRController.CameraHeight < 0.1f)
        {
            yield return new WaitForSeconds(.2f);
        }
        var average = 0f;
        for (int i =0; i < 5; i++)
        {
            average += OVRController.CameraHeight;
            yield return new WaitForSeconds(.2f);
        }
        floorHeight = average / 5;
        var scaler = playerScale / floorHeight;
        ScaleTargets(scaler);
    }

    void ScaleTargets(float scaleMult)
    {
        // Scale VR Rig up/down to appropriate scale
        transform.localScale = Vector3.one * scaleMult;
        OVRController.GetComponent<CharacterController>().height = playerScale;
        //parent & reposition ant head once VR rig is scaled
        AntHead.SetParent(AntHeadParent);
        AntHead.localPosition = new Vector3(0f, 0.0199f, 0.08f);
        AntHead.localRotation = Quaternion.Euler(11.581f, 0f, 0f);


        Debug.Log("floorHeight: " + floorHeight);
        Debug.Log("transform.localScale: " + transform.localScale);

        //once all this is done, unfade screen
        
    }

}
