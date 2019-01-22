using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAvatarBodyController : MonoBehaviour
{
    //use these to have ant body follow head w/out scaling to VR rig
    public GameObject AntBodyAvatar;
    public GameObject AvatarAnchor;

    // Start is called before the first frame update
    void Start()
    {
        // check to ensure we have an assigned OVRController and then scale VR Rig to camera/floor height
        if (AntBodyAvatar == null)
        {
            AntBodyAvatar = GameObject.FindGameObjectWithTag("PlayerBody");
        }

    }

    // Update is called once per frame
    void Update()
    {
        //make antBodyAvatar follow VR Rig
        AntBodyAvatar.transform.position = Vector3.MoveTowards(AntBodyAvatar.transform.position, AvatarAnchor.transform.position, 2.0f);

        // want avatar body to follow VRRig rotation beyond certain point (i.e. I should be able to look back and see my body, but if I turn 
        // and begin moving my body should follow both rotation and position
        AntBodyAvatar.transform.rotation = AvatarAnchor.transform.rotation;

    }
}
