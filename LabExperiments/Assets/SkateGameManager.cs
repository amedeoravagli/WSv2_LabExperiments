using System;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.InputSystem;

public class SkateGameManager : AbstractManager
{
    private LoggerManager logger;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _player;
    [SerializeField] private InputActionReference _pauseMenuAction;
    [SerializeField] private GameObject _uiCountDown;
    [SerializeField] private InputActionReference _startInteraction;
    [SerializeField] private GameObject _pauseVR;
    [SerializeField] private TMP_Text _textVR;

    private bool isVR = false;
    public bool isReadyToStart = false;
    //private GameStatus _state = 0;
    private Vector3 _initialPosition;
    //public GameStatus GameStatus => _state;
    private int _checkpoint = 0;
    public int GameCheckpoints => _checkpoint;
    private bool isTrain;
    public Action<int> manageDoor;
    private GameStatus lastGameStatus;
    // Start is called before the first frame update
    void Start()
    {
        _initialPosition = _player.transform.localPosition;
        GameObject loggerGO = GameObject.FindGameObjectWithTag("Logger");
        logger = loggerGO.GetComponent<LoggerManager>();
        isVR = GameSingleton.Instance._inputType != InputActionType.DESKTOP;
        isTrain = GameSingleton.Instance.isTraining;
        ChangeState(GameStatus.TRAIN);
        _pauseMenuAction.action.performed += OnPauseAndResume; 
        _startInteraction.action.performed += OnStartGame;
    }

    void OnDestroy(){
        _pauseMenuAction.action.performed -= OnPauseAndResume;
        _startInteraction.action.performed -= OnStartGame;
    }
    public void ChangeState(GameStatus status)
    {
        if(logger != null){
            logger.EventLogger(5);
        }
        GameStatus = status;
        switch(status)
        {
            case GameStatus.TRAIN:
            logger.SendEvent("Event:PlayTraining");
                _pauseMenu.SetActive(false);
                if (isVR){
                    _textVR.text = "Pause";
                    _pauseVR.SetActive(false);
                }
                _uiCountDown.SetActive(false);

                break;
            case GameStatus.START: // start state
                //_uiGame.SetActive(false);
                _pauseMenu.SetActive(false);
                if (isVR){
                    _textVR.text = "Pause";
                    _pauseVR.SetActive(false);
                }
                _uiCountDown.SetActive(true);
                break;
            case GameStatus.COUNTDOWN: // countdown state
                _uiCountDown.GetComponent<Countdown>().startCounting();
                break;
            case GameStatus.PLAYING: // playing state
                logger.SendEvent("Event:Play");
                manageDoor.Invoke(1);

                if(_uiCountDown != null){
                    _uiCountDown.SetActive(false);
                }
                /*if(_uiGame != null)
                {
                    _uiGame.SetActive(true);
                }*/
                if(_pauseVR)
                    _pauseVR.SetActive(false);
                _pauseMenu.SetActive(false);
                break;
            case GameStatus.PAUSE: // pause state
                logger.SendEvent("Event:Pause");

                if(_pauseMenu != null){
                    _pauseMenu.SetActive(true);                
                }

                if (isVR){
                    _textVR.text = "Pause";
                    _pauseVR.SetActive(true);
                }
                break;
            case GameStatus.FINISH: // finish state
                logger.SendEvent("Event:Finish");
                _pauseMenu.SetActive(true);
            
                if (isVR){
                    _textVR.text = "Finish";
                    _pauseVR.SetActive(true);
                }
                break;
        }

    }

    private void OnPauseAndResume(InputAction.CallbackContext context)
    {
        if(GameStatus == GameStatus.PAUSE)
        {
            ChangeState(lastGameStatus);  
        }
        else if(GameStatus == GameStatus.PLAYING || GameStatus == GameStatus.TRAIN)
        {
            lastGameStatus = GameStatus;
            ChangeState(GameStatus.PAUSE);
        }
    }
    private void OnStartGame(InputAction.CallbackContext context)
    {
        if(GameStatus == GameStatus.START && isReadyToStart)
            ChangeState(GameStatus.COUNTDOWN);
        
    }
    // Update is called once per frame
    void Update()
    {
        if(GameStatus == GameStatus.COUNTDOWN && _uiCountDown.GetComponent<Countdown>()._countdown < 0)
        {
            ChangeState(GameStatus.PLAYING);
        }
       
    }

    public void RestartFn()
    {
        // re-generate track
        _player.transform.localPosition = _initialPosition;
        
        logger.SendEvent("Event:Finish");
        
        ChangeState(GameStatus.TRAIN);
        
    }

}
