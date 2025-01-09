using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Lofelt.NiceVibrations;

[RequireComponent(typeof(UIDocument))]

public class GDPRPopup : MonoBehaviour
{
    EventSystem _eS;
    private UnityEngine.UIElements.Button _personalizedButton;
    private UnityEngine.UIElements.Button _randomButton;
    [SerializeField] UnityEngine.UI.Button _myDataButton;

    // Start is called before the first frame update
    void Start()
    {
        _eS = GameObject.Find("EventSystem").GetComponent<EventSystem>(); //find the event system

        var root = GetComponent<UIDocument>().rootVisualElement;
        _myDataButton.onClick.AddListener(OnDataButtonPressed); //add listener to the button
        _personalizedButton = root.Q<UnityEngine.UIElements.Button>("Personalized");
        _randomButton = root.Q<UnityEngine.UIElements.Button>("Random");
        _personalizedButton.clicked += OnPersonalizedButtonPressed; //run the event on button pressed
        _randomButton.clicked += OnRandomButtonPressed; //run the event on button pressed

        //if the user consent was never acquired
        if (Advertisements.Instance.UserConsentWasSet() == false) {
            _eS.enabled = false; //disable the event system to allow the UI document to work properly
            gameObject.SetActive(true); //display GDPR consent form
        }

        //otherwise, initialize
        else {
            Advertisements.Instance.Initialize(); //initialize the ads
            gameObject.SetActive(false); //dont display GDPR consent form
        }
    }

    //when the personalized button is pressed
    private void OnPersonalizedButtonPressed() {
        PlayButtonSound(); //play sound and vibration feedback
        Advertisements.Instance.SetUserConsent(true); //set consent to true if personalized button was pressed
        Advertisements.Instance.Initialize(); //initialize ads
        gameObject.SetActive(false); //close the popup
        _eS.enabled = true; //re-enable the event system
    }

    //when the randomized button is pressed
    private void OnRandomButtonPressed() {
        PlayButtonSound(); //play sound and vibration feedback
        Advertisements.Instance.SetUserConsent(false); //set consent to false if random button was pressed
        Advertisements.Instance.Initialize(); //initialize ads
        gameObject.SetActive(false); //close the popup
        _eS.enabled = true; //re-enable the event system
    }

    private void OnDataButtonPressed() {
        RemoveAllListenters();
        PlayButtonSound(); //play sound and vibration feedback
        gameObject.SetActive(true); //show the consent form again
        _eS.enabled = false; //disable the event system to allow the UI document to work properly
        var root = GetComponent<UIDocument>().rootVisualElement;
        _myDataButton.onClick.AddListener(OnDataButtonPressed); //add listener to the button
        _personalizedButton = root.Q<UnityEngine.UIElements.Button>("Personalized");
        _randomButton = root.Q<UnityEngine.UIElements.Button>("Random");
        _personalizedButton.clicked += OnPersonalizedButtonPressed; //run the event on button pressed
        _randomButton.clicked += OnRandomButtonPressed; //run the event on button pressed
    }
    
    private void RemoveAllListenters() {
        _myDataButton.onClick.RemoveAllListeners();
        _personalizedButton.clicked -= OnPersonalizedButtonPressed;
        _randomButton.clicked -= OnRandomButtonPressed;
    }

    private void PlayButtonSound() {
        SoundManagerScript.PlaySound("button");
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }
}
