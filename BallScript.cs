using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class BallScript : MonoBehaviour
{
    ThemeHandlerScript _tHS;
    GameHandlerScript _gHS;
    float _xForce, _yForce, _animationSpeed; //the x and y force of the ball
    Vector2 _originalScale, _scaleTo;
    Color32 _startColor, _flashColor;

    // Start is called before the first frame update
    void Start()
    {
        _tHS = GameObject.FindGameObjectWithTag("themeHandler").GetComponent<ThemeHandlerScript>(); //get the instance of the script
        _startColor = _tHS.GetThemeSecondary(_tHS.GetCurrentThemeIndex());
        _flashColor = Color.white;//Color.white; //set the flash color

        _animationSpeed = 0.3f; //the animation speed (/2) of the ball
        _originalScale = transform.localScale; //the original scale of the ball
        _scaleTo = _originalScale * 1.2f; //set the max scale size to x1.1 the original size

        _gHS = GameObject.FindGameObjectWithTag("gameHandler").GetComponent<GameHandlerScript>(); //set instance of the gamehandler script

        _xForce = 100f; //the x force
        _yForce = 200f; //set the y force 
    }

    //this method is called when the ball is pressed
    public void OnBallPressed(GameObject _ball) {
        if (_ball.transform.position.x > 0) _xForce = -_xForce; //if on the right side of the screen, negate xForce
        _gHS.UpdateScore(1, _ball.transform.position ,new Vector2(_xForce,_yForce)); //update the score
        _ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero; //reset the velocity
        _ball.GetComponent<Rigidbody2D>().AddForce(new Vector2(_xForce, _yForce)); //add the force
        _xForce = Mathf.Abs(_xForce); //set the xforce back to the absolute value
        Scale(_ball); //call the scale animation of the ball
        Flash(_ball.transform.GetChild(0).gameObject); //call the flash animation of the ball
    }

    //scale up the ball
    public void Scale(GameObject ball) {
        ball.transform.DOScale(_scaleTo, 0f) //transform to the scale size
           .SetLoops(0, LoopType.Yoyo) //dont loop again 
           .OnComplete(() => { //when completed the animation
               ball.transform.DOScale(_originalScale, _animationSpeed); //scale back down to the original scale
           });
    }

    //flash the ball
    public void Flash(GameObject flash) {
        var _sR = flash.GetComponent<SpriteRenderer>();
        _sR.DOColor(_flashColor, 0f)
            .OnComplete(() => { //when the flash is completed
                _sR.DOColor(_startColor, _animationSpeed); //fade back to alpha 0
            });
    }

    //this method returns the force
    public Vector2 GetForce() {
        return new Vector2(_xForce, _yForce); //return the force
    }

}
