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
        biteHapticsClip_L = new OVRHapticsClip(biteAudioClip);
        biteHapticsClip = new OVRHapticsClip(10);
        for (int i = 0; i < 10; i ++)
        {
            biteHapticsClip.Samples[i] = i % 2 == 0 ? (byte)0 : (byte)150;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //NOTE THIS IS NOT OWORKING
        if (OVRInput.Get(OVRInput.Button.One))
        {
            mandiblesAnimator.SetTrigger("bite");
            OVRHaptics.RightChannel.Preempt(biteHapticsClip);
            OVRHaptics.LeftChannel.Preempt(biteHapticsClip_L);

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
