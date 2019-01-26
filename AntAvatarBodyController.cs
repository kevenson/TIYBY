using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntAvatarBodyController : MonoBehaviour
{
    //use these to have ant body follow head w/out scaling to VR rig
    public GameObject AntBodyAvatar;
    public GameObject AvatarAnchor;
    public PlayerAnimationController BodyAnimation;
    public Vector3 offset;
    public float allowedHeadRotationDegrees = 30f;
    public float rotationCheckTimer = .2f;
    private float timeCount = 0f; //0.09f;

    // Start is called before the first frame update
    void Start()
    {
        // check to ensure we have an assigned OVRController and then scale VR Rig to camera/floor height
        if (AntBodyAvatar == null)
        {
            AntBodyAvatar = GameObject.FindGameObjectWithTag("PlayerBody");
        }
        StartCoroutine(RotationCheck());

    }

    // Update is called once per frame
    void LateUpdate()
    {
        //make antBodyAvatar follow VR Rig
        AntBodyAvatar.transform.position = AvatarAnchor.transform.position - offset;
    }

    void RotateAvatar(float currentBodyRotation, float rotationAmt)
    {
        // make antbodyAvatar follow rotation of vr rig beyond a specific point
        var currentRot = Quaternion.Euler(0, currentBodyRotation, 0);
        var rot = Quaternion.Euler(0, rotationAmt, 0);

        // trigger animation and rotate body
        BodyAnimation.BodyRotation();
        AntBodyAvatar.transform.rotation = Quaternion.Slerp(currentRot, rot, timeCount);
        timeCount += Time.deltaTime;
        //AntBodyAvatar.transform.rotation = rot;
    }

    IEnumerator RotationCheck()
    {
        // check x times a second (rotationCheckTimer) if AvatarAnchor has rotated x degrees (allowedHeadRotationDegrees)
        // if it has, follow 
        for (; ; ) {
            yield return new WaitForSeconds(rotationCheckTimer);

            var check1 = AvatarAnchor.transform.rotation.eulerAngles.y;
            var check2 = AntBodyAvatar.transform.rotation.eulerAngles.y;
            if (Mathf.Abs(check1 - check2) >= allowedHeadRotationDegrees)
            {
                RotateAvatar(check2, check1);
            }
        }
    }
}
