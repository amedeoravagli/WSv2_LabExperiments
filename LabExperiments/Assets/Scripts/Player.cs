using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public abstract class Player : MonoBehaviour
{

    //[SerializeField] InputActionType inputActionType;
    //[SerializeField] InputActionAsset _actionAsset; // temporary field it'll be declare in the starting phase when username is register
    /*[SerializeField] InputActionReference _movementLR;
    [SerializeField] InputActionReference _movementFB;
    [SerializeField] InputActionReference _movementOrientation;
    
    private InputAction _movementLR;
    private InputAction _movementFB;
    private InputAction _movementOrientation;
    */
    [SerializeField] private AbstractManager _gamesManager;
    public float verticalMove = 0.0f;
    public float horizontalMove = 0.0f;
    public Quaternion horizontalOrientation = Quaternion.identity;
    public Vector2 movementAxisWS;
    //public float horizontalOrientation = 0.0f;
    public float verticalOrientation = 0.0f;
    public bool isMovingFB = false;
    public bool isMovingLR = false;
    private string username = "";
    public bool activeMovement = false;
    private GameSingleton game;
    private PlayerInputActionStruct _playerAction;
    protected BTConnection _gyroscope;
    protected BoditrakCOM _boditrak;
    
    // Start is called before the first frame update
    virtual protected void Start()
    {
        game = GameSingleton.Instance;
        username = game.Username;
        _playerAction = game.GetPlayerInputAction();
        Debug.Log(_playerAction.name + " - " + _playerAction.movementFB.action + " - " + _playerAction.movementLR.action + " - " + _playerAction.playerOrientation + " - ");
        _gyroscope = FindObjectOfType<BTConnection>();
        _boditrak = FindObjectOfType<BoditrakCOM>();

    }  

    // Update is called once per frame
    virtual protected void Update()
    {
        verticalMove = 0;
        horizontalMove = 0;
        isMovingFB = false;
        isMovingLR = false;   
        activeMovement = _gamesManager.GameStatus == GameStatus.TRAIN || _gamesManager.GameStatus == GameStatus.PLAYING ;
        //activeMovement = true;
        if (activeMovement)
        {
            
            if(_gyroscope.isActiveAndEnabled)
            {
                Quaternion angle = _gyroscope.transform.localRotation;
                /*if (Mathf.Abs(angle.y) < 0.05f)
                {
                    angle.y = 0.0f;
                }*/
                horizontalOrientation = angle ;
            }
            else
            {
                if(_playerAction.playerOrientation != null){
                    
                    if(_playerAction.playerOrientation.action.expectedControlType == "Vector2"){
                        horizontalOrientation = Quaternion.Euler(0, _playerAction.playerOrientation.action.ReadValue<Vector2>().x, 0);
                        //verticalOrientation = _playerAction.playerOrientation.action.ReadValue<Vector2>().y;

                    }else{
                        horizontalOrientation = Quaternion.Euler(0,_playerAction.playerOrientation.action.ReadValue<float>(),0);
                        //verticalOrientation = _playerAction.playerOrientation.action.ReadValue<float>();

                    }
                }
            }
            if(_boditrak.isActiveAndEnabled){
                horizontalMove = _boditrak.transform.localPosition.x;
                verticalMove = _boditrak.transform.localPosition.y;
            }else{    
                if(_playerAction.movementFB.action.expectedControlType == "Vector2"){
                    verticalMove = _playerAction.movementFB.action.ReadValue<Vector2>().y;
                }else{
                    verticalMove = _playerAction.movementFB.action.ReadValue<float>();
                }
                if(_playerAction.movementLR.action.expectedControlType == "Vector2"){
                    horizontalMove = _playerAction.movementLR.action.ReadValue<Vector2>().x;
                }else{
                    horizontalMove = _playerAction.movementLR.action.ReadValue<float>();
                }
            }

            //verticalMove = _playerAction.movementFB.action.ReadValue<Vector2>().y;
            //horizontalMove = _playerAction.movementLR.action.ReadValue<Vector2>().x;
            isMovingFB = _playerAction.movementFB.action.IsPressed();
            isMovingLR = _playerAction.movementLR.action.IsPressed();   
        }    
    }

    abstract protected Vector3 movePlayer();
}
    
