using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;


public enum DeviceType{
    KEYBOARD,
    MOUSE,
    XRCONTROLLER,
    HMD
}

[System.Serializable]
public struct PlayerInputActionStruct{
    [SerializeField] public string name;
    //[SerializeField] DeviceType device;
    [SerializeField] public InputActionReference movementLR;
    [SerializeField] public InputActionReference movementFB;
    [SerializeField] public InputActionReference playerOrientation;
}


[System.Serializable]
public class PlayerInputAction{
    public int _playerInputSelected;
    [SerializeField] public List<PlayerInputActionStruct> _playerInputActions;

    public PlayerInputActionStruct GetPlayerInputActionStruct(){
        Assert.IsTrue(_playerInputSelected < _playerInputActions.Count, "playerInputActionSelected doesn't exist: index out of bound");
        if (_playerInputSelected >= _playerInputActions.Count || _playerInputSelected < 0){
            return _playerInputActions[0];
        }
        return _playerInputActions[_playerInputSelected];
    }

}

/*
    Class to manage the choice and all different input: UnityInputSystem, ArduinoGyroscopium
*/
public class PlayerInputManager : MonoBehaviour
{
    public InputActionType inputActionType = 0;
    [SerializeField] private PlayerInputAction inputDesktop;
    [SerializeField] private PlayerInputAction inputDirect;
    [SerializeField] private PlayerInputAction inputIndirect;
    [SerializeField] private PlayerInputAction inputExternal;

    private GameSingleton game;
    private Dictionary<InputActionType,PlayerInputAction> playerActionStructure;

    // Start is called before the first frame update
    void Awake()
    {
        game = GameSingleton.Instance;
        if(game.playerInputManager != null){
            Destroy(this);
        }else{
            game.playerInputManager = this;
        }
        playerActionStructure = new Dictionary<InputActionType, PlayerInputAction>
        {
            { InputActionType.DESKTOP, inputDesktop },
            { InputActionType.DIRECT, inputDirect },
            { InputActionType.INDIRECT, inputIndirect },
            { InputActionType.EXTERNAL, inputExternal }
        };
        game.Init(playerActionStructure);
        game.ChangeInputActionType(inputActionType);
        
        DontDestroyOnLoad(gameObject);


        //_actionMap = game.InputActionMap;
        
    }

    public void ChangeInputActionInteractionType(InputActionType type){
        inputActionType = type;
    }

    public List<string> GetNameInputDeviceByType(InputActionType type)
    {
        List<string> result = new();
        
        foreach(var inputaction in playerActionStructure[type]._playerInputActions){
            result.Add(inputaction.name);
        }
        
        return result;
    }

}
