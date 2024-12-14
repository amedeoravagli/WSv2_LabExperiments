using UnityEngine;
using System;
//using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Rendering.PostProcessing;


public class MovePlayer : Player
{
    //private const float DETECTION_TH = 0.2f;
    //[SerializeField] private InputMode _mode = InputMode.Click;
    [SerializeField] private GameObject _track;
    [SerializeField] private GameObject _postProcessGO;
    [SerializeField] private float _walkSpeed = 7;

    public bool Blocked
    {
        get { return _blocked; }
        set
        {
            if (value && !_blocked)
            {
                //_bigArrow.gameObject.SetActive(false);
                _lastSpeed = 0;
                _lastDir = Vector3.zero;
                _factor = 0;
                //_rightPadDown = false;
                //_leftPadDown = false;
            }

            _blocked = value;
        }
    }

    public float SlidingSpeed => _walkSpeed;


    bool _blocked = false;

    //[Space] [SerializeField] Transform _bigArrow;
    CharacterController _target;
    //bool _leftPadDown = false;
    //bool _rightPadDown = false;
    float _factor = 0;
    float _factorForward = 0.5f;
    float _smoothStep = 5.0f;
    float _speedStep = 2.0f;
    float _lastSpeed = 0;
    Vector3 _lastDir = Vector3.zero;
    //float t;
    Vector3 moveDirection;
    private PostProcessVolume _postProcess;
    private float _postProcessFactor = 0.0f;
    //private float _postProcSpeed = 2.0f;
    public float _slowSpeed = 3.0f;
    public float _fastSpeed = 7.0f;
    private float _trackSpeed = 5.0f;
    private Player _player;
    public float TrackSpeed
    {
        get { return _trackSpeed; }
        private set
        {
            _trackSpeed = value;
        }
    }
    override protected void Start()
    {
        base.Start();
        _postProcess = _postProcessGO.GetComponent<PostProcessVolume>();
        _target = GetComponent<CharacterController>();
        _player = GetComponent<Player>();
    }

    override protected void Update()
    {   
        base.Update();

        float verticalInput = 0.5f + (_player.verticalMove / 2);
        if (_player.isMovingFB)
        {
            _factorForward += Time.deltaTime * _speedStep * Mathf.Sign(_player.verticalMove);
            if (_player.verticalMove > 0.80f){
                _postProcessFactor += Time.deltaTime * _speedStep;
            }else {
                _postProcessFactor -= Time.deltaTime * _speedStep;
            }
            if(_postProcessFactor > 1.0f){
                _postProcessFactor = 1.0f;
            }
            if(_postProcessFactor < 0.0f){
                _postProcessFactor = 0.0f;
            }
            if (_factorForward > 1.0f){
                _factorForward = 1.0f;
            }   
            if (_factorForward < 0.0f){
                _factorForward = 0.0f;
            }
            
        }
        else
        {
            if (_factorForward <= 0.45f || _factorForward >= 0.55f)
            {
                //movement = _lastDir * (Time.deltaTime * _factor * _lastSpeed);
                _factorForward -= Time.deltaTime * _speedStep * Mathf.Sign(_factorForward - 0.5f);
                //if (_factor < 0)
                //    _factor = 0;
            }
            if(_player.verticalMove <= 0.80f)
                _postProcessFactor -= Time.deltaTime * _speedStep/2;
            if (_postProcessFactor < 0){
                _postProcessFactor = 0;
            }
        }
       
        //float _posZVolume = Mathf.Lerp(-2, 0, _postProcessFactor); 
        //_postProcess.transform.localPosition = new Vector3(0,0,_posZVolume);
        if(_player.activeMovement){
            _postProcess.weight = _postProcessFactor;
            TrackSpeed = Mathf.Lerp(_slowSpeed, _fastSpeed, _factorForward);
            //_track.GetComponentsInChildren<TrackMotion>();
            foreach (Transform t_child in _track.transform)
            {
                if (t_child.CompareTag("SubTrack"))
                {
                    t_child.transform.localPosition -= new Vector3(0,0,Time.deltaTime * TrackSpeed);
                }
            }
            moveDirection = Vector3.zero;
            if (_target.isGrounded)
                moveDirection = CalculateLRMotion();
            //moveDirection = ApplyGravity(moveDirection);
            _target.SimpleMove(moveDirection);    
        }
        
    }

    private Vector3 ApplyGravity(Vector3 moveDirection)
    {
        moveDirection.y -= 9.81f * Time.deltaTime;
        return moveDirection;
    }

    private Vector3 CalculateLRMotion()
    {
        Vector3 movement = Vector3.zero;
        float offset = 0.05f;
        float hMove = _player.horizontalMove;

        //Debug.Log(hMove);
        if (hMove < -offset)
        {
            _lastDir = -Vector3.right;
            //if (Input.GetKey(KeyCode.LeftShift))
            //    _lastSpeed = RunSpeed;
            //else
            //{
                //tp = _input.LeftPadAxis;
                //t = (tp.padY + 1) / 2;
                //_lastSpeed = Mathf.Lerp(0, _walkSpeed, t);
            _lastSpeed = SlidingSpeed;
            //}
            
            movement = _lastSpeed * hMove /** Time.deltaTime */* Vector3.right;
            _factor += Time.deltaTime * _smoothStep;
            if (_factor > 1)
                _factor = 1;
        }
        else if (hMove > offset)
        {
            _lastDir = Vector3.right;
            
            //tp = _input.LeftPadAxis;
            //t = (tp.padY + 1) / 2;
            //_lastSpeed = Mathf.Lerp(0, _walkSpeed, t);
            _lastSpeed = SlidingSpeed;
        

            movement = _lastSpeed * hMove /** Time.deltaTime */ * Vector3.right;
            _factor += Time.deltaTime * _smoothStep;

            if (_factor > 1)
                _factor = 1;
        }
        else if (_factor > 0)
        {
            _factor -= Time.deltaTime * _smoothStep;
            movement = _factor * _lastSpeed  /** Time.deltaTime */ * _lastDir; 
            if (_factor < 0)
                _factor = 0;
        }
        else if (_lastSpeed > 0)
        {
            _lastSpeed = 0;
            _lastDir = Vector3.zero;
        }

        return movement;
    }

    protected override Vector3 movePlayer()
    {
        throw new NotImplementedException();
    }
}
