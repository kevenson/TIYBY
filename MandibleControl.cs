using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MandibleControl : MonoBehaviour
{
    // 
    public Animator mandiblesAnimator;
    public AudioClip biteAudioClip;
    private OVRHapticsClip biteHapticsClip;
    public GameObject demoFoodItem;
    public GameObject pickedUpFood;
    public static bool carryingFood = false;
    private bool droppingFood = false;

    void Start()
    {
        biteHapticsClip = new OVRHapticsClip(biteAudioClip);
        demoFoodItem.SetActive(false);
        pickedUpFood.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        //Check for button presses
        //if (OVRInput.Get(OVRInput.Button.One))
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.4f)
        {

            activateMandibles();
        }
    }

    private void activateMandibles()
    {
        if (carryingFood == false)
        {
            mandiblesAnimator.SetTrigger("bite");
            OVRHaptics.LeftChannel.Mix(biteHapticsClip);
            OVRHaptics.RightChannel.Mix(biteHapticsClip);
        }
        else
        {
            //var newFood = Instantiate(pickedUpFood, demoFoodItem.transform.position, demoFoodItem.transform.rotation);
            //var newFoodRigid = newFood.GetComponent<Rigidbody>();
            //newFood.SetActive(true);
            //newFoodRigid.isKinematic = false;
            //demoFoodItem.SetActive(false);  // set original to false
            //pickedUpFood.transform.position = demoFoodItem.transform.position;
            //pickedUpFood.SetActive(true);
            //pickedUpFood.GetComponent<Rigidbody>().isKinematic = false;
            //carryingFood = false;
            droppingFood = true;
            StartCoroutine("DropFood");
        }
    }

    IEnumerator DropFood()
    {
        demoFoodItem.SetActive(false);  // set original to false
        pickedUpFood.transform.position = demoFoodItem.transform.position;
        pickedUpFood.SetActive(true);
        pickedUpFood.GetComponent<Rigidbody>().isKinematic = false;
        yield return new WaitForSeconds(0.5f);
        droppingFood = false;
        carryingFood = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Food" && carryingFood == false && droppingFood == false)
        {
            other.gameObject.SetActive(false);
            demoFoodItem.SetActive(true);
            carryingFood = true;
        }

    }

}
