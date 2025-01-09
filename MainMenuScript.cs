using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.SocialPlatforms;
using Lofelt.NiceVibrations;
using System;

public class MainMenuScript : MonoBehaviour
{
    Color32 _secondaryColor;
    ThemeHandlerScript _tHS; 
    [SerializeField] Canvas _configCanvas, _statCanvas;
    [SerializeField] Button _playButton, _configButton, _statsButton, _settingsButton, _themeButton, _backButton, _backButton2, _privacyButton;
    [SerializeField] TMP_Text _titleText,_configTitleText, _statsTitleText, _playButtonText, _configButtonText, _settingsButtonText, _themeButtonText, _backButtonText , _bestScoreText, _gamesPlayedText, _juggledText;
    [SerializeField] ParticleSystem _particles;
    bool _wasThemeSet;

    private void Awake() {
        try {
            _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //get the instance of the global
            _secondaryColor = _tHS.GetThemeSecondary(PlayerPrefs.GetInt("theme", 0));
            Colorize(); //colorize the main menu
            _wasThemeSet = true;
        }
        catch (NullReferenceException ex) {
            _wasThemeSet = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!_wasThemeSet) {
            _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //set the instance of the script
            _secondaryColor = _tHS.GetThemeSecondary(PlayerPrefs.GetInt("theme", 0)); //get the saved theme index
            Colorize(); //colorize the main menu
        }
        _configCanvas.enabled = false; //disable the config canvas
        _statCanvas.enabled = false; //disable the stat canvas
        SetStats(); //set the stats on the stats screen
        if (GlobalGameHandlerScript._adCounter == 2){
            GlobalGameHandlerScript._adCounter = 0; //reset the variable
            Advertisements.Instance.ShowInterstitial();
        }
    }

    //this method colors all the items on the main menu
    private void Colorize() {
        var _main = _particles.main;
        _main.startColor = new ParticleSystem.MinMaxGradient(_secondaryColor); //set the particles start color to the secondary color

        _playButton.GetComponent<Image>().color = _secondaryColor;
        _configButton.GetComponent<Image>().color = _secondaryColor;
        _statsButton.GetComponent<Image>().color = _secondaryColor;
        _settingsButton.GetComponent<Image>().color = _secondaryColor;
        _themeButton.GetComponent<Image>().color = _secondaryColor;
        _backButton.GetComponent<Image>().color = _secondaryColor;
        _backButton2.GetComponent<Image>().color = _secondaryColor;
        _privacyButton.GetComponent<Image>().color = _secondaryColor;

        _titleText.color = _secondaryColor;
        _configTitleText.color = _secondaryColor;
        _statsTitleText.color = _secondaryColor;
        _playButtonText.color = _secondaryColor;
        _configButtonText.color = _secondaryColor; 
        _settingsButtonText.color = _secondaryColor;
        _themeButtonText.color = _secondaryColor;
        _backButtonText.color = _secondaryColor;
        _bestScoreText.color = _secondaryColor;
        _gamesPlayedText.color = _secondaryColor;
        _juggledText.color = _secondaryColor;
    }

    //this method sets the stats in the stat menu
    private void SetStats() {
        _bestScoreText.text = "BEST SCORE:\n" + PlayerPrefs.GetInt("highScore", 0); //set the highscore text
        _gamesPlayedText.text = "GAMES PLAYED:\n" + PlayerPrefs.GetInt("gamesPlayed", 0); //set the gamesplayed text
        _juggledText.text = "JUGGLES:\n"+PlayerPrefs.GetInt("juggles", 0); //set the juggle text
    }

    //when the play button is pressed
    public void  OnPlayButtonPressed() {
        SceneManager.LoadScene("ingame"); //load the ingame scene
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the config button is pressed
    public void OnConfigButtonPressed() {
        _configCanvas.enabled = true; //enable the config canvas
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the stats button is pressed
    public void OnStatsButtonPressed() {
        _statCanvas.enabled = true;
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the theme button is pressed
    public void OnThemeButtonPressed() {
        SceneManager.LoadScene(sceneName: "themeMenu"); //load the thememenu
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the settings button is pressed
    public void OnOptionsButtonPressed() {
        SceneManager.LoadScene(sceneName: "optionsMenu"); //load the options menu
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the back button on the config menu is pressed
    public void OnBackButtonPressed() {
        if (_configCanvas.enabled) _configCanvas.enabled = false; //disable the configCanvas
        if (_statCanvas.enabled) _statCanvas.enabled = false; //disable the statsCanvas
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }
}
