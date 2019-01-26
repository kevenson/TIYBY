using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationAudio : MonoBehaviour
{
    // simple class to play audiosource associated with animation. 
    // to use just attach to gameobject being animated, and attach
    // PlayAudio() method as an animation event in the Animation window

    private AudioSource audioToPlay;

    // Start is called before the first frame update
    void Start()
    {
        audioToPlay = GetComponent<AudioSource>();
    }
    void PlayAudio()
    {
        audioToPlay.Play();
    }


}
