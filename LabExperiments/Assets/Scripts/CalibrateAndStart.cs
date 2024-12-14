using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class CalibrateAndStart : MonoBehaviour
{
    [SerializeField] private InputActionReference _startInteraction;
    [SerializeField] private GameObject _countdown;
    //private string _message = "";
    //private SessionManager _gamesManager;

    void Awake()
    {
        // hmd calibration step

    }

    // Start is called before the first frame update
    void Start()
    {
        //_gamesManager.isGamePaused = true;
        //_message = "Calibration.. Press a button when you are in position and ready";
        _startInteraction.action.performed += OnStartGame;
    }

    private void OnStartGame(InputAction.CallbackContext context)
    {
        // get hmd position
        
    }
    
}
