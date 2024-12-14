using System.Collections.Generic;
using UnityEngine.Assertions;

public enum InputActionType{
    DESKTOP,
    DIRECT,
    INDIRECT,
    EXTERNAL
}

public class GameSingleton 
{
    #region [Singleton]
    private static GameSingleton _instance = null;
    public static GameSingleton Instance
    {
        get
        {
            _instance ??= new GameSingleton();
            return _instance;
        }
    } 

    #endregion

    public IBTConnection bluetooth = null;
    public PlayerInputManager playerInputManager = null;
    public LoggerManager loggerManager = null;
    public InputActionType _inputType = 0;
    private Dictionary<InputActionType, PlayerInputAction> _playerActionStructure = null;
    private bool activeLogger = false;
    public bool isTraining;
    private string _username = "";
    public string Username
    {
        get{
            return _username;
        }
        set{
            _username = value;
        }
    }

    public void Init(Dictionary<InputActionType, PlayerInputAction> playerActionStructure){
        Assert.IsNotNull(playerActionStructure, "playerActionStructure is null or not valid");
        if (playerActionStructure != null)
            _playerActionStructure = playerActionStructure;
    }    

    public PlayerInputActionStruct GetPlayerInputAction(){
        //PlayerInputActionStruct res = _playerActionStructure[_indexAsset].GetPlayerInputActionStruct();
        return _playerActionStructure[_inputType].GetPlayerInputActionStruct();
    }
    public void ChangeInputActionType(InputActionType iat){
        _inputType = iat;
    } 

    public void SetActiveLogger(bool active){
        activeLogger = active;
    }

    public bool GetActiveLogger()
    {
        return activeLogger;
    }

    public void ChooseInputDevice(int index)
    {
        _playerActionStructure[_inputType]._playerInputSelected = index;
    }
    
}
