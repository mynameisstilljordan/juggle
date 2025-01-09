using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeHandlerScript : MonoBehaviour
{
    GlobalGameHandlerScript _gHS; 
    Camera _mainCamera;
    int _currentTheme;

    Color32[] _colors = new Color32[] {
        new Color32(255,77,77,255), //light red 0 
        new Color32(191,58,58,255), //dark red 1

        new Color32(255, 153, 51, 255), //light orange 2
        new Color32(191,115,38, 255), //dark orange 3

        new Color32(255, 255, 77, 255), //light yellow 4
        new Color32(191, 191, 58, 255), //dark yellow 5

        new Color32(77, 255, 77, 255), //light green 6
        new Color32(58, 191, 58, 255), //dark green 7
                                       
        new Color32(77, 77, 255, 255), //light blue 8
        new Color32(58, 58, 191, 255), //dark blue 9

        new Color32(153, 51, 255, 255), //light purple 10
        new Color32(115, 38, 191, 255), //dark purple 11

        new Color32(255, 128, 255, 255), //light pink 12
        new Color32(191, 96, 191, 255), //dark pink 13

        new Color32(255, 227, 179, 255), //light tan 14
        new Color32(217, 193, 152, 255), //dark tan 15

        new Color32(250, 250, 250, 255), //light white 16
        new Color32(217, 217, 217, 255), //dark white 17

        new Color32(108, 65, 45, 255), //light brown 18
        new Color32(79, 48, 33, 255), //dark brown 19

        new Color32(44, 44, 44, 255), //light black 20
        new Color32(26, 26, 26, 255), //dark black 21

        new Color32(77,136,255,255), //light cyan 22
        new Color32(58,102,191,255) //dark cyan 23
    };

    Color32[,] _themes;

    string[] _themeNames = new string[] {
        "CHERRY",
        "ORANGE",
        "LEMON",
        "LIME",
        "BLUEBERRY",
        "BLUE RASPBERRY",
        "GRAPE",
        "STRAWBERRY",
        "CANTALOUPE",
        "SNOWBERRY",
        "COCOA BEAN",
        "BLACKBERRY",
        "APPLE",
        "PLUM",
        "KIWI",
        "WATERMELON",
        "PUMPKIN",
        "BANANA",
        "DRAGONFRUIT",
        "ACKEE",
        "PINEAPPLE",
        "PEAR",
        "MANGO",
        "PASSIONFRUIT",
        "AVACADO",
        "PEACH",
        "LANGSAT",
        "MANGOSTEEN",
        "COCONUT",
        "TOMATO",
        "PHYSALIS",
        "EGGPLANT",
        "RAMBUTAN",
        "SOURSOP",
        "LONGAN",
        "MAMEY SAPOTE",
        "SANTOL",
        "JENIPAPO",
        "JACKFRUIT",
        "PAPAYA",
        "GUAVA",
        "FIG",
        "AKEBI",
        "DECAISNEA FARGESII",
        "BLACK MUSHROOM"

    };
    Color32[] _lockedThemes = new Color32[] {
        new Color32(154,154,154,255), //primary (lighter) gray
        new Color32(107,107,107,255) //secondary (darker) gray
    };

    private void Awake() {
        _themes = new Color32[,]{
    { _colors[0], _colors[1] }, //cherry
    { _colors[2], _colors[3] }, //orange
    { _colors[4], _colors[5] }, //lemon
    { _colors[6], _colors[7]}, //lime
    { _colors[8], _colors[9]}, //blueberry
    { _colors[22], _colors[23]}, //blue raspberry
    { _colors[10], _colors[11]}, //grape
    { _colors[12], _colors[13]}, //strawberry
    { _colors[14], _colors[15]}, //cantaloupe
    { _colors[16], _colors[17]}, //snowberry
    { _colors[18], _colors[19]}, //cocoa bean
    { _colors[20], _colors[21]}, //blackberry
    { _colors[14], _colors[0]}, //apple
    { _colors[10], _colors[14]}, //plum
    { _colors[6], _colors[18]}, //kiwi
    { _colors[0], _colors[6]}, //watermelon
    { _colors[2], _colors[14]}, //pumpkin
    { _colors[15], _colors[4]}, //banana
    { _colors[16], _colors[12] }, //dragonfruit
    { _colors[4], _colors[21] }, //ackee
    {_colors[18], _colors[4] }, //pineapple
    {_colors[14], _colors[18] }, //pear
    {_colors[2], _colors[4] }, //mango
    {_colors[10], _colors[4] }, //passionfruit
    {_colors[14], _colors[6]}, //avacado
    {_colors[2], _colors[0] }, //peach
    {_colors[16], _colors[14] }, //langsat
    {_colors[16], _colors[10] }, //mangosteen
    {_colors[18], _colors[16] }, //coconut
    {_colors[2], _colors[6] }, //tomato
    {_colors[4], _colors[6] }, //physalis
    {_colors[10], _colors[6] }, //eggplant
    {_colors[0], _colors[16] }, //rambutan
    {_colors[6], _colors[16] }, //soursop
    {_colors[16], _colors[20] }, //longan
    {_colors[18], _colors[2] }, //mamey sapote
    {_colors[2], _colors[16] }, //santol
    {_colors[18], _colors[20] }, //jenipapo
    {_colors[6], _colors[4] }, //jackfruit
    {_colors[2], _colors[20] }, //papaya
    {_colors[6], _colors[12] }, //guava
    {_colors[10], _colors[0] }, //fig
    {_colors[10], _colors[22] }, //akebi
    {_colors[8], _colors[20] }, //decaisnea fargesii
    {_colors[20], _colors[14] } //black fungus mushroom
    };
    }

    // Start is called before the first frame update
    void Start()
    {
        _gHS = GameObject.FindGameObjectWithTag("globalHandler").GetComponent<GlobalGameHandlerScript>(); //get the instance of the globalgamehandlerscript
        _currentTheme = PlayerPrefs.GetInt("theme", 0); //get the saved theme index
    }

    // Update is called once per frame
    void Update()
    {
        if (_mainCamera == null) Colorize(); //if there's no reference to the main camera (called after every scene change)
    }

    //this method colorizes the background to appear as the chosen theme
    void Colorize() {
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>(); //find the main camera and save an instance of its script
        if (_gHS.GetActiveScene() != "themeMenu") { //if not in the theme menu
            _mainCamera.backgroundColor = GetThemePrimary(_currentTheme); //set the background to the current primary theme
        }
    }

    //this method returns the requested theme's primary color
    public Color32 GetThemePrimary(int _index) {
        if (PlayerPrefs.GetInt("invertColors", 0) != 1) { //if the invert theme option is not toggled
            return _themes[_index, 0]; //return the primary color of the theme
        }
        else return _themes[_index, 1]; //otherwise, return the secondary color of the theme
    }

    //this method returns the requested theme's secondary color
    public Color32 GetThemeSecondary(int _index) {
        if (PlayerPrefs.GetInt("invertColors", 0) != 1) { //if the invert theme option is not toggled
            return _themes[_index, 1]; //return the secondary color of the theme
        }
        else return _themes[_index, 0]; //otherwise, return the primary color of the theme
    }

    //this method returns the primary locked color
    public Color32 GetThemeLockedPrimary() {
        return _lockedThemes[0]; //return the locked color 
    }

    //this method returns the secondary locked color
    public Color32 GetThemeLockedSecondary() {
        return _lockedThemes[1]; //return the locked color 
    }

    //this method returns the current theme index
    public int GetCurrentThemeIndex() {
        return _currentTheme; //return the current theme index
    }

    //this method returns the current theme name
    public string GetCurrentThemeName() {
        return _themeNames[_currentTheme]; //return the _currentTheme index of _themeNames
    }

    public void SetCurrentThemeIndex(int _newIndex) {
        _currentTheme = _newIndex; //set the current theme to the new index
    }

    //this method returns the amount of rows in the theme array
    public int AmountOfThemes() {
        return _themes.GetLength(0); //return the number of rows
    }

    //this method saves the current theme index to playerprefs
    public void SaveCurrentTheme() {
        PlayerPrefs.SetInt("theme", _currentTheme); //update the saved variable to the current theme
    }

    //this method checks to see if the parameter theme has been unlocked and returns the boolean
    public bool IsThemeUnlocked(int _theme) {
        int _totalScore = PlayerPrefs.GetInt("totalScore", 0); //load the total score (amount of times the player has juggled a ball in total)
        //switch statement determining if the player has unlocked the theme
        int _maxThemeFromRequirements = -1; //the index of the highest theme we can use
        int _requirements = 0; //this variable validates each of the requirement checks
        
        for (int i = 0; i < AmountOfThemes(); i++) {
            if (_totalScore >= _requirements) _maxThemeFromRequirements++; //if the current (i) requirement has been met, increase the indexcount of the theme the player is able to use
            else break; //if the current requirements aren't met, break
            _requirements += 50 * ( i + 1); //increment the requirements by the requirements incrementer
        }
        return _maxThemeFromRequirements >= _theme; //return the comparrison
    }

    //this method returns the required juggle count minus the current juggle count 
    public int WhenDoIUnlockThisTheme() {
        int _totalScore = PlayerPrefs.GetInt("totalScore", 0); //load the total score (amount of times the player has juggled a ball in total)
        //switch statement determining if the player has unlocked the theme
        int _maxThemeFromRequirements = -1; //the index of the highest theme we can use
        int _requirements = 0; //this variable validates each of the requirement checks

        for (int i = 0; i < AmountOfThemes(); i++) {
            if (_totalScore >= _requirements) _maxThemeFromRequirements++; //if the current (i) requirement has been met, increase the indexcount of the theme the player is able to use
            else { //if player hasn't unlocked the current theme yet
                return _requirements - _totalScore; //return how many more total juggles are needed in order to unlock the theme
            }
            _requirements += 50 * (i + 1);
        }

        return 0; //if code made it to this point (which means player has all themes), return 0
    }
}
