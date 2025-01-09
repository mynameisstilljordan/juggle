using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBrainScript : MonoBehaviour
{
    GameHandlerScript _gHS;
    // Start is called before the first frame update
    void Start()
    {
        _gHS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<GameHandlerScript>();
    }

    //when the ball falls out of the map
    private void OnBecameInvisible() {
        gameObject.SetActive(false);
        _gHS.GameOver(); //end the game
    }
}
