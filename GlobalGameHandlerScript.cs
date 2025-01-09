using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lofelt.NiceVibrations;
using UnityEngine.Events;

public class GlobalGameHandlerScript : MonoBehaviour
{
    public static GameObject _instance; //the instance var
    public static int _adCounter;
    string _npaValue;
    private void Awake() {
        DontDestroyOnLoad(this.gameObject); //dont destroy the gameobject on load
        if (_instance == null) _instance = gameObject; //if there isnt an existing instance of the gameobject
        else Destroy(gameObject); //destroy the gameobject
    }

    private void Start() {
        Application.targetFrameRate = 30; //set fps at 45
        _adCounter = 0;

        Advertisements.Instance.Initialize(); //initialize ads

        if (PlayerPrefs.GetInt("touchVibration", 1) == 0) HapticController.hapticsEnabled = false; //disable haptics
        else HapticController.hapticsEnabled = true; //enable haptics
    }

    //this method gets the name of the active scene and returns it
    public string GetActiveScene() {
        return SceneManager.GetActiveScene().name; //return the name of the active scene
    }
}
