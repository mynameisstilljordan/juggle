using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip pop, tap, button;
    static AudioSource audioSrc;
    // Start is called before the first frame update
    void Start()
    {
        pop = Resources.Load<AudioClip>("pop");
        tap = Resources.Load<AudioClip>("tap");
        button = Resources.Load<AudioClip>("button");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void PlaySound(string clip) {
        switch (clip) {
            case "pop":
                audioSrc.PlayOneShot(pop);
                break;
            case "tap":
                audioSrc.PlayOneShot(tap);
                break;
            case "button":
                audioSrc.PlayOneShot(button);
                break; 
        }
    }
}
