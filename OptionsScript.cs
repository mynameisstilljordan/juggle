using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lofelt.NiceVibrations;
using UnityEngine.SceneManagement;
using TMPro;

public class OptionsScript : MonoBehaviour
{
    [SerializeField] TMP_Text _title, _touchTitle, _soundTitle, _touchOff, _touchOn, _soundOff, _soundOn;
    [SerializeField] Sprite[] _toggleButtons;
    [SerializeField] Button _vibrationButton, _soundButton, _backButton;
    ThemeHandlerScript _tHS;
    Color32 _secondaryColor;

    private void Awake() {
        _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //get the theme handler script reference
        _secondaryColor = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //get the secondary color
        Colorize(); //color the elements on the screen
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("touchVibration", 1) == 0) _vibrationButton.image.sprite = _toggleButtons[0]; //set the vibration button to off 
        else _vibrationButton.image.sprite = _toggleButtons[1]; //set the vibration button to on
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 0) _soundButton.image.sprite = _toggleButtons[0];
        else _soundButton.image.sprite = _toggleButtons[1];

    }

    private void Colorize() {
        _title.color = _secondaryColor;
        
        _touchTitle.color = _secondaryColor;
        _vibrationButton.GetComponent<Image>().color = _secondaryColor; 
        _touchOff.color = _secondaryColor;
        _touchOn.color = _secondaryColor; 

        _soundTitle.color = _secondaryColor;
        _soundButton.GetComponent<Image>().color = _secondaryColor;
        _soundOff.color = _secondaryColor;
        _soundOn.color = _secondaryColor;

        _backButton.GetComponent<Image>().color = _secondaryColor;
    }

    //when the vibration toggle button is pressed
    public void VibrationToggleButtonPressed() {

        if (PlayerPrefs.GetInt("touchVibration",1) == 0) { //if the vibration is off
            HapticController.hapticsEnabled = true; //enable haptics
            PlayerPrefs.SetInt("touchVibration", 1); //turn the vibration on
            _vibrationButton.image.sprite = _toggleButtons[1]; //turn to on switch image
        }

        else { //if the vibration is on
            HapticController.hapticsEnabled = false; //disable haptics
            PlayerPrefs.SetInt("touchVibration", 0); //turn the vibration off
            _vibrationButton.image.sprite = _toggleButtons[0]; //turn to off switch image
        }

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    public void SFXToggleButtonPressed() {
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 0) { //if the vibration is off
            PlayerPrefs.SetInt("sfxEnabled", 1); //turn the vibration on
            _soundButton.image.sprite = _toggleButtons[1]; //turn to on switch image
        }

        else { //if the vibration is on
            PlayerPrefs.SetInt("sfxEnabled", 0); //turn the vibration off
            _soundButton.image.sprite = _toggleButtons[0]; //turn to off switch image
        }

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    public void OnMoreOptionsButtonPressed() {
        
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }


    //when the back button is pressed
    public void OnBackButtonPressed() {
        SceneManager.LoadScene(sceneName: "mainMenu"); //load the main menu
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }
}
