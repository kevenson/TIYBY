using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPheromoneSystem : MonoBehaviour
{
    private ParticleSystem partSys;
    public AudioClip pheromoneDetectClip;
    private OVRHapticsClip pheromoneDetectHapticsClip;
    //public bool biteableItem = false;

    // Start is called before the first frame update
    void Start()
    {
        partSys = GetComponentInChildren<ParticleSystem>();
        //partSys.emission(false);
        if (pheromoneDetectClip == null) { return; }
        else { pheromoneDetectHapticsClip = new OVRHapticsClip(pheromoneDetectClip); }
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Just entered collider: " + other);
        if (other.tag == "Antennae_L" || other.tag == "Antennae_R")
        {
            if (OVRInput.Get(OVRInput.Axis1D.SecondaryIndexTrigger) > 0.1f)
            {
                partSys.Play();
                // trigger haptic pulse on controller
                if (other.tag == "Antennae_L") { OVRHaptics.LeftChannel.Mix(pheromoneDetectHapticsClip); }
                else { OVRHaptics.RightChannel.Mix(pheromoneDetectHapticsClip); }
            }
        }
        //if (other.tag == "Mandibles")
        //{
        //    biteableItem = true;
        //}

    }

}
