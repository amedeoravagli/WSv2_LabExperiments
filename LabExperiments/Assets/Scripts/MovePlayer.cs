using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
//using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Assertions;

public class MovePlayer : MonoBehaviour
{
    //private const float DETECTION_TH = 0.2f;
    //[SerializeField] private InputMode _mode = InputMode.Click;
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
    float _smoothStep = 0.05f;
    float _lastSpeed = 0;
    Vector3 _lastDir = Vector3.zero;
    //float t;
    Vector3 moveDirection;

    void Awake()
    {
        _target = GetComponent<CharacterController>();
        
    }

    void Update()
    {
        if (!Blocked)
        {
            moveDirection = Vector3.zero;
            if (_target.isGrounded)
                moveDirection = CalculateMotion();
            moveDirection = ApplyGravity(moveDirection);
            _target.Move(moveDirection);
        }
    }

    private Vector3 ApplyGravity(Vector3 moveDirection)
    {
        moveDirection.y -= 9.81f * Time.deltaTime;
        return moveDirection;
    }

    private Vector3 CalculateMotion()
    {
        Vector3 movement = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.A))
        {
            _lastDir = -Vector3.right;
            if (Input.GetKeyDown(KeyCode.LeftShift))
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
        else if (Input.GetKeyDown(KeyCode.D))
        {
            _lastDir = Vector3.right;
            if (Input.GetKeyDown(KeyCode.LeftShift))
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

    /*public float _slidingSpeed = 8.0f;
    
    // Update is called once per frame

    void InteractionController()
    {
       
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localPosition += new Vector3(-_slidingSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.localPosition += new Vector3(_slidingSpeed * Time.deltaTime, 0, 0);
        }
    }
    */
}
