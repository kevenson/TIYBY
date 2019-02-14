using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipSwitching : MonoBehaviour
{
    // switch between regular, ambient background noises
    public AudioClip[] clips;
    private AudioSource a_source;
    public int clip_delay = 5;

    void Start()
    {
        a_source = GetComponent<AudioSource>();
        StartCoroutine("PlayAudio");
    }

    IEnumerator PlayAudio()
    {
        for (; ; )
        {
            for (int i =0; i < clips.Length; i++)
            {
                a_source.clip = clips[i];
                a_source.Play();
                yield return new WaitForSeconds(clip_delay);
            }
        }
    }
}
