using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Lofelt.NiceVibrations;

public class ThemeMenuScript : MonoBehaviour
{
    int _currentTheme;
    ThemeHandlerScript _tHS;
    [SerializeField] TMP_Text _themeTitle, _unlockText, _themeScoreText, _invertColorText; //the text that tells the user when they unlock a theme
    [SerializeField] Button _leftButton, _rightButton, _useButton, _invertColorButton;
    [SerializeField] Image _themeImage;
    
    // Start is called before the first frame update
    void Awake()
    {
        _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //set the instance of the script
        _currentTheme = _tHS.GetCurrentThemeIndex(); //get the current theme index from the themehandler
        UpdateThemeUnlockRequirementsUI(); //update any theme menu ui elements that need to be updated
        NavigationButtonCheck(); //check if any navigation buttons need to be enabled or disabled
    }

    //when the use button is pressed
    public void OnUseButtonPressed() {
        _tHS.SaveCurrentTheme(); //save the current theme
        SceneManager.LoadScene(sceneName: "mainMenu"); //go back to the main menu
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the left button is pressed
    public void OnLeftButtonPressed() {
        if (_tHS.GetCurrentThemeIndex() > 0) _tHS.SetCurrentThemeIndex(_tHS.GetCurrentThemeIndex() - 1); //if greater than 0, decrement the current index
        NavigationButtonCheck(); //check if any navigation buttons need to be enabled or disabled
        UpdateThemeUnlockRequirementsUI(); //update any theme menu ui elements that need to be updated
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //when the right button is pressed
    public void OnRightButtonPressed() {
        if (_tHS.GetCurrentThemeIndex() < _tHS.AmountOfThemes()) _tHS.SetCurrentThemeIndex(_tHS.GetCurrentThemeIndex() + 1); //if greater than 0, decrement the current index
        NavigationButtonCheck(); //check if any navigation buttons need to be enabled or disabled
        UpdateThemeUnlockRequirementsUI(); //update any theme menu ui elements that need to be updated
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //this method updates everything on screen to reflect whether or not the current theme is unlocked
    public void UpdateThemeUnlockRequirementsUI() {
        _themeScoreText.text = (_tHS.GetCurrentThemeIndex() + 1).ToString(); //set the score on the example to the theme number
        if (_tHS.IsThemeUnlocked(_tHS.GetCurrentThemeIndex())) { //if the current theme is unlocked
            _themeTitle.text = _tHS.GetCurrentThemeName(); //update the title text to the current theme name
            _useButton.enabled = true; //enable the "use" button
            _useButton.interactable = true; //grey out the button
            _invertColorButton.GetComponent<Image>().enabled = true;
            _invertColorButton.enabled = true;
            _invertColorButton.interactable = true;
            _invertColorText.text = "INVERT\nCOLORS";
            _unlockText.text = ""; //set unlock text to nothing
            UpdateColors(true); //update colors for an unlocked theme
        }
        else { //if the theme hasn't been unlocked
            _themeTitle.text = "???"; //placeholder text for locked theme
            _useButton.enabled = false; //disable the "use" button
            _useButton.interactable = false; //grey out the button
            _invertColorButton.GetComponent<Image>().enabled = false;
            _invertColorButton.enabled = false;
            _invertColorButton.interactable = false;
            _invertColorText.text = "";
            if (_tHS.GetCurrentThemeIndex() > 0 && _tHS.IsThemeUnlocked(_tHS.GetCurrentThemeIndex()-1)) //if the previous theme is unlocked, say how many juggles are required for the next one
                _unlockText.text = "THIS THEME UNLOCKS IN:\n" + _tHS.WhenDoIUnlockThisTheme() + " MORE JUGGLES"; //set unlock text to requirements
            else _unlockText.text = "THIS THEME UNLOCKS IN:\n??? MORE JUGGLES"; //otherwise, don't tell the player how many more juggles they need
                UpdateColors(false); //update colors for a locked theme
        }
    }

    //this method updates the colors on screen to reflect the current color array index
    public void UpdateColors(bool _unlocked) {
        if (_unlocked) { //if the current theme is unlocked
            _themeTitle.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color text to the secondary color
            _themeImage.color = _tHS.GetThemePrimary(_tHS.GetCurrentThemeIndex()); //set the sample theme image to the primary color  
            _useButton.image.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the use button to the secondary color
            _themeScoreText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the example theme text to the secondary color
            _invertColorButton.image.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the invert button to the secondary color
            _invertColorText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the invert text to the secondary color
        }
        else { //if the current theme is not unlocked
            _themeImage.color = _tHS.GetThemeLockedPrimary(); //set the sample theme image to the primary color
            _themeTitle.color = _tHS.GetThemeLockedSecondary(); //set the theme title to the secondary locked color
            _useButton.image.color = _tHS.GetThemeLockedSecondary(); //set the use button to the secondary locked color
            _themeScoreText.color = _tHS.GetThemeLockedSecondary(); //set the example theme text to the secondary color
            _invertColorButton.image.color = _tHS.GetThemeLockedSecondary(); //set the invert button to the secondary locked color
            _invertColorText.color = _tHS.GetThemeLockedSecondary(); //set the invert text to the secondary color
        }
    }

    //this method checks if either nagivation button needs to be disabled and acts accordingly
    public void NavigationButtonCheck() {
        if (_tHS.GetCurrentThemeIndex() == 0) _leftButton.GetComponent<Image>().enabled = false; //if on the first index of themes
        else _leftButton.GetComponent<Image>().enabled = true; //otherwise, keep on the left button
        if (_tHS.GetCurrentThemeIndex() == _tHS.AmountOfThemes() -1) _rightButton.GetComponent<Image>().enabled = false; //if on the last index of themes
        else _rightButton.GetComponent<Image>().enabled = true; //otherwise, keep on the right button
    }

    //when the invert colors button is pressed
    public void InvertColorsButtonPressed() {
        if (PlayerPrefs.GetInt("invertColors") == 0) { //if the invert colors option is off
            PlayerPrefs.SetInt("invertColors", 1); //turn it on
        }
        else { //if the invert colors option is on
            PlayerPrefs.SetInt("invertColors", 0); //turn it off
        }
        UpdateColors(_tHS.IsThemeUnlocked(_tHS.GetCurrentThemeIndex())); //update the colors
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }
}
