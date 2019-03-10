using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCounter : MonoBehaviour
{
    public static int foodScore = 0;
    public static bool winState = false;
    // Start is called before the first frame update

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Food" && MandibleControl.carryingFood == false)
        {
            foodScore += 1;
            other.gameObject.SetActive(false);
            if(foodScore >=2)
            {
                winState = true;
            }
        }
    }

}
