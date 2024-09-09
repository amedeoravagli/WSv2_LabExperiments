using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GamesManager : MonoBehaviour
{
    [SerializeField] private GameObject _timeGO; // canvas text of time left 
    [SerializeField] private GameObject _uiPanel;
    [SerializeField] private GameObject _uiScore;
    [SerializeField] private GameObject _player;

    public float _startingTime = 60.0f;
    public bool isGamePaused = false;
    public bool isGameFinished = false;
    private float _leftTime = 0.0f;
    private TMP_Text _timeText;
    private TMP_Text _finishScoreText;


    // Start is called before the first frame update
    void Start()
    {
        _timeText = _timeGO.GetComponent<TMP_Text>();
        _leftTime = _startingTime;
        _finishScoreText = _uiScore.GetComponent<TMP_Text>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameFinished)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isGamePaused = !isGamePaused;
                if (isGamePaused)
                {
                    // stop game display restart and exit button
                    Time.timeScale = 0;
                    _finishScoreText.text = _player.GetComponent<PlayerScoreManager>()._score.ToString();
                    _uiPanel.SetActive(true);
                }else {
                    Time.timeScale = 1;
                    _uiPanel.SetActive(false);
                }
            }
            if (!isGamePaused)
            {
                _leftTime -= Time.deltaTime;
                if (_leftTime <= 0){
                    _leftTime = 0;
                    _timeText.text = _leftTime.ToString("0.00");
                    isGameFinished = true;
                    // stop game display restart and exit button
                    Time.timeScale = 0;
                    _finishScoreText.text = _player.GetComponent<PlayerScoreManager>()._score.ToString();
                    _uiPanel.SetActive(true);
                }
            }
            
            _timeText.text = _leftTime.ToString("0.00");
        }
        
    }

    public void ExitFn()
    {
        // save users points on file 

        // close app 
        Application.Quit();
    }

    public void RestartFn()
    {
        // re-generate track

        // reset time scale game to 1 
        Time.timeScale = 1;

        // close pause menu 
        isGamePaused = false;
        isGameFinished = false;
        _leftTime = _startingTime;
        _player.GetComponent<PlayerScoreManager>()._score = 0;
        _uiPanel.SetActive(false);
        
    }

}
