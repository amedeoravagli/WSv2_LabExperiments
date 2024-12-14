using UnityEngine;
using UnityEngine.UI;

public class StartMenuUIFunctions : MonoBehaviour
{
    [SerializeField] private Text textDisplayBT; 
    [SerializeField] private TMPro.TMP_Dropdown deviceDropdown;
    [SerializeField] private TMPro.TMP_Dropdown inputDropdown;
    private GameSingleton game;
    void Start()
    {
        game = GameSingleton.Instance;
        inputDropdown.value = 0;
        deviceDropdown.AddOptions(game.playerInputManager.GetNameInputDeviceByType(InputActionType.DESKTOP));
        deviceDropdown.value = 0;
        ChooseDevice(0);
        ChangeTraining(false);
    }

    void Update()
    {
        if(textDisplayBT != null )
        {
            if (game.bluetooth != null && game.bluetooth.IsConnected())
                textDisplayBT.text = "Bluetooth: Connected";
            else{
                textDisplayBT.text = "Bluetooth: Not Connected";

            }
        }
    }
    

    public void ShowDevices(int index)
    {
        if(deviceDropdown == null)
            Debug.Log("dropdown is null");
        deviceDropdown.ClearOptions();
        switch(index){
            case 0:
                deviceDropdown.AddOptions(game.playerInputManager.GetNameInputDeviceByType(InputActionType.DESKTOP));
                game.ChangeInputActionType(InputActionType.DESKTOP);
                break;
            case 1:
                deviceDropdown.AddOptions(game.playerInputManager.GetNameInputDeviceByType(InputActionType.DIRECT));
                game.ChangeInputActionType(InputActionType.DIRECT);
                break;
            case 2:
                deviceDropdown.AddOptions(game.playerInputManager.GetNameInputDeviceByType(InputActionType.INDIRECT));
                game.ChangeInputActionType(InputActionType.INDIRECT);

                break;
            case 3:
                deviceDropdown.AddOptions(game.playerInputManager.GetNameInputDeviceByType(InputActionType.EXTERNAL));
                game.ChangeInputActionType(InputActionType.EXTERNAL);

                break;
            default:
                break;
        }
    }
    public void SetActiveLogger(bool active){
        game.SetActiveLogger(active);
    }

    public void ChooseDevice(int index){
        game.ChooseInputDevice(index);
    }

    public void ChangeTraining(bool isTraining)
    {
        game.isTraining = isTraining;
    }

    public void EnableBTConnection(bool isEnabled)
    {
        FindObjectOfType<BTConnection>().enabled = isEnabled;
    }

}
