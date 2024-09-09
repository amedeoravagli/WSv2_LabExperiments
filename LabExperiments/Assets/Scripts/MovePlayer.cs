using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
//using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;
using UnityEngine.Rendering.PostProcessing;
using Unity.VisualScripting;

public class MovePlayer : MonoBehaviour
{
    //private const float DETECTION_TH = 0.2f;
    //[SerializeField] private InputMode _mode = InputMode.Click;
    [SerializeField] private GameObject _track;
    [SerializeField] private GameObject _postProcessGO;
    [SerializeField] private float _walkSpeed = 5;
    [SerializeField] private float _runSpeed = 8;

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

    public float WalkSpeed => _walkSpeed;

    public float RunSpeed => _runSpeed;

    bool _blocked = false;

    //[Space] [SerializeField] Transform _bigArrow;
    CharacterController _target;
    //bool _leftPadDown = false;
    //bool _rightPadDown = false;
    float _factor = 0;
    float _factorForward = 0.5f;
    float _smoothStep = 0.05f;
    float _lastSpeed = 0;
    Vector3 _lastDir = Vector3.zero;
    //float t;
    Vector3 moveDirection;
    private PostProcessVolume _postProcess;
    private float _postProcessFactor = 0.0f;
    private float _postProcSpeed = 2.0f;
    public float _slowSpeed = 3.0f;
    public float _fastSpeed = 7.0f;
    private float _trackSpeed = 5.0f;
    public float TrackSpeed
    {
        get { return _trackSpeed; }
        private set
        {
            _trackSpeed = value;
        }
    }
    void Awake()
    {
        _postProcess = _postProcessGO.GetComponent<PostProcessVolume>();
        _target = GetComponent<CharacterController>();
        
    }

    void Update()
    {
        // manage speed of the track
        if (Input.GetKey(KeyCode.W)) // speed up
        {
            if (_postProcessFactor < 1)
                _postProcessFactor += Time.deltaTime * _postProcSpeed; 
            
            _factorForward += _smoothStep;
            if (_factorForward > 1.0f){
                _factorForward = 1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.S)) // slow down
        {
            _factorForward -= _smoothStep;
            if (_factorForward < 0.0f){
                _factorForward = 0.0f;
            }
        }
        else if (_factorForward <= 0.45f || _factorForward >= 0.55f)
        {
            //movement = _lastDir * (Time.deltaTime * _factor * _lastSpeed);
            _factorForward -= _smoothStep * Mathf.Sign(_factorForward - 0.5f);
            //if (_factor < 0)
            //    _factor = 0;
        }
        
        if (!Input.GetKey(KeyCode.W)) // speed up
        {
            if (_postProcessFactor > 0)
                _postProcessFactor -= Time.deltaTime * _postProcSpeed/2;
        }
        
        if (_postProcessFactor > 1)
        {
            _postProcessFactor = 1;
        }
        else if (_postProcessFactor < 0)
        {
            _postProcessFactor = 0;
        }
        //float _posZVolume = Mathf.Lerp(-2, 0, _postProcessFactor); 
        //_postProcess.transform.localPosition = new Vector3(0,0,_posZVolume);
        _postProcess.weight = _postProcessFactor;
        TrackSpeed = Mathf.Lerp(_slowSpeed, _fastSpeed, _factorForward);
        _track.GetComponentsInChildren<TrackMotion>();
        foreach (Transform t_child in _track.transform){
            if (t_child.CompareTag("SubTrack")){
                t_child.transform.localPosition -= new Vector3(0,0,Time.deltaTime * TrackSpeed);
            }
        }
        moveDirection = Vector3.zero;
        if (_target.isGrounded)
            moveDirection = CalculateMotion();
        moveDirection = ApplyGravity(moveDirection);
        _target.Move(moveDirection);
    
    }

    private Vector3 ApplyGravity(Vector3 moveDirection)
    {
        moveDirection.y -= 9.81f * Time.deltaTime;
        return moveDirection;
    }

    private Vector3 CalculateMotion()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            _lastDir = -Vector3.right;
            if (Input.GetKey(KeyCode.LeftShift))
                _lastSpeed = RunSpeed;
            else
            {
                //tp = _input.LeftPadAxis;
                //t = (tp.padY + 1) / 2;
                //_lastSpeed = Mathf.Lerp(0, _walkSpeed, t);
                _lastSpeed = WalkSpeed;
            }

            movement = _lastDir * (Time.deltaTime * _factor * _lastSpeed);
            _factor += _smoothStep;
            if (_factor > 1)
                _factor = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _lastDir = Vector3.right;
            if (Input.GetKey(KeyCode.LeftShift))
                _lastSpeed = RunSpeed;
            else
            {
                //tp = _input.LeftPadAxis;
                //t = (tp.padY + 1) / 2;
                //_lastSpeed = Mathf.Lerp(0, _walkSpeed, t);
                _lastSpeed = WalkSpeed;
            }

            movement = _lastDir * (Time.deltaTime * _factor * _lastSpeed);
            _factor += _smoothStep;
            if (_factor > 1)
                _factor = 1;
        }
        else if (_factor > 0)
        {
            movement = _lastDir * (Time.deltaTime * _factor * _lastSpeed);
            _factor -= _smoothStep;
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
}
