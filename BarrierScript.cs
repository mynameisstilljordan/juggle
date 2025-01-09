using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    //on collision
    private void OnCollisionEnter2D(Collision2D other) {
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1 && other.transform.CompareTag("ball")) SoundManagerScript.PlaySound("tap"); //if collided with a ball, play the ball tap effect
    }
}
