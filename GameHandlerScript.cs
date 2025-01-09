using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Lofelt.NiceVibrations;
using MoreMountains.Feedbacks;

public class GameHandlerScript : MonoBehaviour
{
    [SerializeField] MMF_Player _ballPressed; 
    [SerializeField] GameObject _bottomLeftBarrierCorner; //the bottomleft border
    [SerializeField] GameObject _topRightBarrierCorner; //the topright border
    [SerializeField] GameObject _ball; //the ball instance
    [SerializeField] TMP_Text _scoreText; //the score text 
    [SerializeField] TMP_Text _endgameScoreText, _endgameHighscoreText, _gameOverText, _tooltipText;
    [SerializeField] Canvas _endgameCanvas; //the endgame canvas
    [SerializeField] Button _nextButton; //the next button
    ThemeHandlerScript _tHS;
    GlobalGameHandlerScript _gGHS;
    BallScript _bS; 
    public int _score, _juggles, _totalJuggles; //the score for the game
    bool _hasGameEnded, _isTouchVibrationEnabled;
    
    // Start is called before the first frame update
    void Start()
    {
        GlobalGameHandlerScript._adCounter++;
        _hasGameEnded = false; //set the value of this bool to false by default
        _totalJuggles = PlayerPrefs.GetInt("juggles", 0); //load the playerpref for juggles
        _bS = GetComponent<BallScript>();
        _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //get the instance of the script
        _endgameCanvas.enabled = false; //disable the endgame canvas
        SetupBoundaries(); //setup the room boundaries depending on screen size
        var _ballInstance = Instantiate(_ball, new Vector2(0, 0), Quaternion.identity); //spawn a ball in the middle of the room
        _ballInstance.GetComponent<SpriteRenderer>().color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the ball to the current secondary color
        Colorize(); //color everything ingame
        UpdateGamesPlayed(); //update the number of games played
    }

    // Update is called once per frame
    void Update() {
        foreach (Touch _touch in Input.touches) { //for each touch on the screen
            if (_touch.phase == TouchPhase.Began) { //if the touch began
                Ray _touchposition = Camera.main.ScreenPointToRay(new Vector3(_touch.position.x, _touch.position.y, 0f)); //convert the touch position to an on screen location
                RaycastHit2D _hit = Physics2D.GetRayIntersection(_touchposition); //get the ray intersection of the touch and the object

                if (_hit && _hit.collider.tag == "ball"){ //if a ball was hit
                    if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("pop"); //if the sound is enabled, play it
                    HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
                    _bS.OnBallPressed(_hit.transform.gameObject);
                    _juggles++; //increment juggles
                }
            }
        }
    }
    
    //this method colors all the ingame objects to reflect the current theme
    void Colorize() {
        _scoreText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the score text to the current secondary color
        _gameOverText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the text to the current secondary color
        _endgameScoreText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the text to the current secondary color
        _endgameHighscoreText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the text to the current secondary color
        _tooltipText.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the text to the current secondary color
        _nextButton.image.color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the button to the current secondary color
    }


    void SetupBoundaries() {
        Vector3 _point;

        //place topright
        _point = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane));
        _topRightBarrierCorner.transform.position = _point; //move the barrier to point

        //place bottomleft
        _point = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        _bottomLeftBarrierCorner.transform.position = _point; //move the barrier to point
    }

    //this method is called when the game must end
    public void GameOver() {
        if (!_hasGameEnded) { //if the game hasnt ended already
            _hasGameEnded = true; //mark this as true to avoid it being called more than once
            UpdateHighscore(); //update the highscore
            UpdateJuggles(); //update the juggle count
            if (GameObject.FindGameObjectWithTag("endgameCanvas") != null) _endgameCanvas.enabled = true; //enable the endgame canvas
            _endgameScoreText.text = "SCORE: " + _scoreText.text; //set the score text
            _endgameHighscoreText.text = "BEST: " + PlayerPrefs.GetInt("highScore", 0); //update the highscore text
            PlayerPrefs.SetInt("totalScore", PlayerPrefs.GetInt("totalScore") + _score); //add the score to the total score   
        }
    }

    //this method updates the score (called everytime a ball is tapped)
    public void UpdateScore(int _amount, Vector3 _tappedBallPosition,Vector2 _velocity) {

        if (_score == 0) _tooltipText.enabled = false; //destroy the tooltip text after the first juggle

        _score += _amount; //increment the score by the requested amount
        _scoreText.text = _score.ToString(); //set the score text to the string
        _ballPressed.PlayFeedbacks(); //play the ball pressed feedback

        //if the current score is evenly divisible by 9
        if ((_score % 10) == 0) {
            var _newBallVelocity = new Vector2(_velocity.x * -1, _velocity.y); //invert the x velocity, carry the y velocity
            var _newBallInstance = ObjectPool.Instance.GetBall();
            _newBallInstance.SetActive(true);
            _newBallInstance.transform.position = _tappedBallPosition;
            _newBallInstance.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //reset the velocity for the new ball instance
            _newBallInstance.GetComponent<Rigidbody2D>().AddForce(_newBallVelocity); //apply the force of the new velocity
            _newBallInstance.GetComponent<SpriteRenderer>().color = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex()); //set the color of the ball
            _bS.Flash(_newBallInstance);
            _bS.Scale(_newBallInstance);
        }
    }

    //when the next button on the endgame panel is pressed
    public void NextButtonPressed() {
        SceneManager.LoadScene(sceneName: "mainMenu");
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact); //light haptic effect
        if (PlayerPrefs.GetInt("sfxEnabled", 1) == 1) SoundManagerScript.PlaySound("button"); //if the sound is enabled, play it
    }

    //this method returns the current score
    public int GetScore() {
        return _score; //return the score
    }

    //check for highscore and update it 
    private void UpdateHighscore() {
        if (_score > PlayerPrefs.GetInt("highScore", 0)) {
            PlayerPrefs.SetInt("highScore", _score); //if the current score is greater than the previous highscore, update it
        }
    }

    //check for juggles and update
    public void UpdateJuggles() {
        PlayerPrefs.SetInt("juggles",_totalJuggles + _juggles); //add the current juggles on to the total juggles
    }

    //check for games played and update
    public void UpdateGamesPlayed() {
        PlayerPrefs.SetInt("gamesPlayed", PlayerPrefs.GetInt("gamesPlayed", 0) + 1); //add 1 to the total games played
    }
}
