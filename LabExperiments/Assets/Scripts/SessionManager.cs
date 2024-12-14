using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
public enum GameStatus{
    TRAIN,
    START,
    COUNTDOWN,
    PLAYING,
    PAUSE,
    FINISH
}
public class SessionManager : AbstractManager
{
    private LoggerManager logger;
    [SerializeField] private GameObject _timeGO; // canvas text of time left 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _uiScore;
    [SerializeField] private GameObject _player;
    [SerializeField] private InputActionReference _pauseMenuAction;
    [SerializeField] private GameObject _uiCountDown;
    [SerializeField] private InputActionReference _startInteraction;
    [SerializeField] private GameObject _uiGame;
    [SerializeField] private GameObject _pauseVR;
    [SerializeField] private TMP_Text _scoreVR;
    [SerializeField] private TMP_Text _textVR;

    [SerializeField] private GameObject _trackGenerator;
    public float _startingTime = 300.0f;
    private float _leftTime = 0.0f;
    private TMP_Text _timeText;
    private TMP_Text _finishScoreText;
    private bool isVR = false;
    private Vector3 _initialPosition;
    private PlayerScoreManager _playerScoreManager;
    private bool isTrain;
    // Start is called before the first frame update
    void Start()
    {
        _timeText = _timeGO.GetComponent<TMP_Text>();
        _leftTime = _startingTime;
        _finishScoreText = _uiScore.GetComponent<TMP_Text>();
        _initialPosition = _player.transform.localPosition;
        _playerScoreManager = _player.GetComponent<PlayerScoreManager>();
        GameObject loggerGO = GameObject.FindGameObjectWithTag("Logger");
        logger = loggerGO.GetComponent<LoggerManager>();
        isVR = GameSingleton.Instance._inputType != InputActionType.DESKTOP;
        isTrain = GameSingleton.Instance.isTraining;
        ChangeState(GameStatus.START);

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
            case GameStatus.START: // start state
                _leftTime = _startingTime;
                _timeText.text = _startingTime.ToString("0.00");

                _uiGame.SetActive(false);
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

                if(_uiCountDown != null){
                    _uiCountDown.SetActive(false);
                }
                if(_uiGame != null)
                {
                    _uiGame.SetActive(true);
                }
                if(_pauseVR)
                    _pauseVR.SetActive(false);
                _pauseMenu.SetActive(false);
                break;
            case GameStatus.PAUSE: // pause state
                logger.SendEvent("Event:Pause");

                _finishScoreText.text = _playerScoreManager._score.ToString();
                if(_pauseMenu != null){
                    _pauseMenu.SetActive(true);                
                }

                if (isVR){
                    _scoreVR.text = _playerScoreManager._score.ToString();
                    _textVR.text = "Pause";
                    _pauseVR.SetActive(true);
                }
                break;
            case GameStatus.FINISH: // finish state
                logger.SendEvent("Event:Finish");
                _finishScoreText.text = _playerScoreManager._score.ToString();
                _pauseMenu.SetActive(true);
            
                if (isVR){
                    _scoreVR.text = _playerScoreManager._score.ToString();
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
            ChangeState(GameStatus.PLAYING);  
        }
        else if(GameStatus == GameStatus.PLAYING)
        {
            ChangeState(GameStatus.PAUSE);
        }
    }
    private void OnStartGame(InputAction.CallbackContext context)
    {
        if(GameStatus == GameStatus.START)
            ChangeState(GameStatus.COUNTDOWN);
        
    }
    // Update is called once per frame
    void Update()
    {
        if(GameStatus == GameStatus.PLAYING)
        {
            if(isTrain){
                _leftTime -= Time.deltaTime;
                if(_leftTime <= 0)
                {
                    _leftTime = 0;
                    ChangeState(GameStatus.FINISH);
                }
                _timeText.text = _leftTime.ToString("0.00");
            }
            else{
                _timeText.text = " ";
            }   
        }
        else if(GameStatus == GameStatus.COUNTDOWN && _uiCountDown.GetComponent<Countdown>()._countdown < 0)
        {
            ChangeState(GameStatus.PLAYING);
        }
       
    }

    public void RestartFn()
    {
        // re-generate track
        _player.transform.localPosition = _initialPosition;
        _playerScoreManager._score = 0;
        for(int i = 0; i < _trackGenerator.transform.childCount; i++)
        {
            Destroy(_trackGenerator.transform.GetChild(i).gameObject);
        }
        _trackGenerator.GetComponent<GenerateTrack>().RestartSubTrack();
        logger.SendEvent("Event:Finish");
        
        ChangeState(GameStatus.START);
        
    }


}

public abstract class AbstractManager : MonoBehaviour
{
    public GameStatus GameStatus;

}