using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteAnimation : MonoBehaviour
{
    // 
    public Animator mandiblesAnimator;
    public AudioClip biteAudioClip;
    private OVRHapticsClip biteHapticsClip;

    void Start()
    {
        biteHapticsClip = new OVRHapticsClip(biteAudioClip);
    }


    // Update is called once per frame
    void Update()
    {
        //Check for button presses
        //if (OVRInput.Get(OVRInput.Button.One))
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.1f)
        {
            mandiblesAnimator.SetTrigger("bite");
            OVRHaptics.LeftChannel.Mix(biteHapticsClip);
            OVRHaptics.RightChannel.Mix(biteHapticsClip);

        }

    }

}