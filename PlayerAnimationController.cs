using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // 
    public Animator antennaAnimator;
    public Animator mandiblesAnimator;
    public Animator bodyAnimator;
    public AudioClip biteAudioClip;
    private OVRHapticsClip biteHapticsClip;
    private OVRHapticsClip biteHapticsClip_L;

    public float idleDelay = 5.0f;
    //public bool AntBodyTurning = false;


    void Start()
    {
        StartCoroutine("AntennaeController");
        StartCoroutine("DoMovementCheck");
        biteHapticsClip = new OVRHapticsClip(biteAudioClip);

    }


    // Update is called once per frame
    void Update()
    {
        //Check for button presses
        if (OVRInput.Get(OVRInput.Button.One))
        {
            mandiblesAnimator.SetTrigger("bite");
            OVRHaptics.LeftChannel.Mix(biteHapticsClip);
            OVRHaptics.RightChannel.Mix(biteHapticsClip);

        }
        
    }

    IEnumerator DoMovementCheck()
    {
        // check to see if we are moving a few times per second, 
        // if we are, call body animation method
        for (; ;)
        {
            MovementCheck();
            yield return new WaitForSeconds(.1f);
        }
    }

    public void BodyRotation()
    {
        bodyAnimator.SetTrigger("bodyRotate");
        //Debug.Log("Should be rotating w/ animation");
    }

    private void MovementCheck()
    {
        // this should check velocity of parent rigidbody to determine movement
        // movement should then trigger body animations
        return;
    }

    IEnumerator AntennaeController()
    {
        for ( ;; ) {
            yield return new WaitForSeconds(idleDelay);
            antennaAnimator.SetTrigger("Antennae_idle");
        }
    }
}
