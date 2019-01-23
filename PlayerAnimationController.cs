using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator antennaAnimator;
    public Animator mandiblesAnimator;
    public float idleDelay = 5.0f;

    void Start()
    {
        StartCoroutine("AntennaeController");
    }

    // Update is called once per frame
    void Update()
    {

        if (OVRInput.Get(OVRInput.Button.One))
        {
            mandiblesAnimator.SetTrigger("bite");
        }
        
    }

    IEnumerator AntennaeController()
    {
        for ( ;; ) {
            yield return new WaitForSeconds(idleDelay);
            antennaAnimator.SetTrigger("Antennae_idle");
        }
    }
}
