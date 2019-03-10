using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaling : MonoBehaviour
{
    private float floorHeight;
    public float playerScale = 1.8f;
    public OVRPlayerController OVRController;
    public Transform AntHeadParent;
    public GameObject AntHead;
    [Tooltip("AntAvatar1.1 = p:0,-1.97f,-1.59f; r:0,0,0")]
    public Vector3 localPosAntHead = new Vector3(0f, 0.0199f, 0.08f);
    public Vector3 localRotAntHead = new Vector3(11.581f, 0f, 0f);
    public bool setPlayerControllerHeight = false;
    //public PlayerScaling_LoadScreen loadScreenScale;
    public bool usingLoadScreenScaling = true;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(52.0f, 15)
        AntHead.SetActive(false);

        if (OVRController == null)
        {
            return;
        }
        else
        {
            // get height if we're not using height from loading screen
            if (!usingLoadScreenScaling) { StartCoroutine(GetHeight()); }
            else { ScaleTargets(PlayerScaling_LoadScreen.scaler); }

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
        var localScaler = playerScale / floorHeight;
        ScaleTargets(localScaler);
    }

    void ScaleTargets(float scaleMult)
    {
        // Scale VR Rig up/down to appropriate scale
        transform.localScale = Vector3.one * scaleMult;
        if (setPlayerControllerHeight) { OVRController.GetComponent<CharacterController>().height = playerScale; }
        
        //parent & reposition ant head once VR rig is scaled
        AntHead.transform.SetParent(AntHeadParent);
        AntHead.transform.localPosition = localPosAntHead;
        AntHead.transform.localRotation = Quaternion.Euler(localRotAntHead);
        AntHead.SetActive(true);


        //Debug.Log("floorHeight: " + floorHeight);
        //Debug.Log("transform.localScale: " + transform.localScale);

        //once all this is done, unfade screen
        
    }

}
