using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;
using UnityEngine.InputSystem;
using Unity.VisualScripting;
using System.Linq;



[System.Serializable]
public class LoggerDevice
{
    [SerializeField] public string DeviceName;
    [SerializeField] public Transform _transform;
    [SerializeField] public bool globalPosition = false;
    [SerializeField] public bool localPosition = false;
    [SerializeField] public bool globalRotation = false;
    [SerializeField] public bool localRotation = false;

    public bool inputAction = false;
    public InputActionReference movementLR;
    public InputActionReference movementFB;
    public InputActionReference playerOrientation;

    //[SerializeField] private bool scale = false;
    //[SerializeField] private bool isRotEuler = false;
    private List<float[]> logBuffer;
    private float baseTime;
    public LoggerDevice(){
        logBuffer = new List<float[]>();
        
        /*
        foreach (var input in actions){
            input.action.action.performed += (InputAction.CallbackContext context) => SaveLogInBuffer(input.code); 
        }
        */
    }

    public void SaveLogInBuffer(float time, int status, int codeInterrupt = -1)
    {
        if (inputAction){
            logBuffer.Add(GetInputValues(time, status, codeInterrupt));
        }else{
            logBuffer.Add(GetDeviceValues(time, status, codeInterrupt));

        }
    }

    public string GetHeader(float baseTime){
        string header = "";
        baseTime = Time.time;
        
        header += DeviceName+"; "+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff")+" ;\n";
        if (inputAction){
            header += "timestamp; status;interrupt;MovementLR; ; ;"+"MovementFB; ; ;"+"PlayerOrientation; ; ;"+"\n";
            header += "msec;"+" Game Status;"+" code;"+"x; y; ;"+"x; y; ;"+"x; y; ;"+"\n";
        }else{
            header += "timestamp; status;interrupt;globalPosition; ; ;"+"localPosition; ; ;"+"globalRotation; ; ; ;"+"localRotation; ; ; ;"+"\n";
            header += "msec;"+" Game Status;"+" code;"+"x; y; z;"+"x; y; z;"+"x; y; z; w;"+"x; y; z; w;"+"\n";  
        }
       
        return header;
    }

    private float[] GetInputValues(float time, int status, int code)
    {
        float [] values = new float[12];
        values[0] = time - baseTime;
        values[1] = (float)status;
        values[2] = (float)code;
        if (movementLR != null)
        {
            if (movementLR.action.expectedControlType == "Vector2")
            {
                values[3] = movementLR.action.ReadValue<Vector2>().x;
                values[4] = movementLR.action.ReadValue<Vector2>().y;
            }else{
                values[3] = movementLR.action.ReadValue<float>();
                values[4] = 0;
            }
            values[5] = 0;
        }
        if (movementLR != null)
        {
            if (movementFB.action.expectedControlType == "Vector2")
            {
                values[6] = movementFB.action.ReadValue<Vector2>().x;
                values[7] = movementFB.action.ReadValue<Vector2>().y;
            }else{
                values[6] = 0;
                values[7] = movementFB.action.ReadValue<float>();
            }
            values[8] = 0;
        }
        if (playerOrientation != null)
        {
            if (movementFB.action.expectedControlType == "Vector2")
            {
                values[9] = movementFB.action.ReadValue<Vector2>().x;
                values[10] = movementFB.action.ReadValue<Vector2>().y;
            }else{
                values[9] = movementFB.action.ReadValue<float>();
                values[10] = 0;
            }
            values[11] = 0;
        }
        return values;
    }

    private float[] GetDeviceValues(float time, int status, int code)
    {
        float [] values = new float[17];
        
        values[0] = time - baseTime;
        values[1] = (float)status;
        values[2] = (float)code;
        if(globalPosition){
            values[3] = _transform.position.x;
            values[4] = _transform.position.y;
            values[5] = _transform.position.z;
        }
        if(localPosition){
            values[6] = _transform.localPosition.x;
            values[7] = _transform.localPosition.y;
            values[8] = _transform.localPosition.z;
        }
        if(globalRotation){
            values[9] = _transform.rotation.x;
            values[10] = _transform.rotation.y;
            values[11] = _transform.rotation.z;
            values[12] = _transform.rotation.w;
        }
        if(localRotation){
            values[13] = _transform.localRotation.x;
            values[14] = _transform.localRotation.y;
            values[15] = _transform.localRotation.z;
            values[16] = _transform.localRotation.w;
        }
        return values;
    }

    public List<string> GetLines()
    {
        List<string> result = new List<string>();
        
        foreach(var values in logBuffer){
            string line = "";
            for (int i = 0; i < values.Length; i++){
                string value = values[i].ToString();
                if (i == 2 && values[i] == -1){
                    value = "";
                }
                line += value + " ;";
            }
            
            line += "\n";
            result.Add(line);
        }
        logBuffer.Clear();
        return result;
    }
    
}

public class LoggerManager : MonoBehaviour
{
    [SerializeField] private List<LoggerDevice> _loggerDevices;
    private TimeSynchronization timeSynchronization;
    private string infoSyncMessage = "";
    private bool infoSended = false;
    private PlayerInputActionStruct _inputDevices;
    private Dictionary<string,FileStream> _fileDescriptors;
    private GameSingleton game;
    private readonly string pathDirLog = ".\\Assets\\SessionLogFiles\\";

    [SerializeField] private int batchLogLine = 1000;
    [SerializeField] AbstractManager sessionManager;
    private int counterLogLine = 0;
    void Awake()
    {
        game = GameSingleton.Instance;
        _inputDevices = game.GetPlayerInputAction();
        Debug.Log(_inputDevices);
        string daystring =  DateTime.Today.Date.Day.ToString() + DateTime.Today.Date.Month.ToString() + DateTime.Today.Date.Year.ToString();
        string hourstring =  DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString(); 
        string pathUserLog = pathDirLog + game.Username + "_Log_" + daystring + "_" + hourstring + "\\";
        string username = game.Username;
        Debug.Log("_"+username + "_ numero caratteri (Length): "+ username.Length);
        if(username.Length <= 1){
            username = "anon";
        }
        infoSyncMessage = username + "_timelog_" + daystring + "_" + hourstring;
        Debug.Log(infoSyncMessage);
        if(game.GetActiveLogger())
        {
            
            Directory.CreateDirectory(pathUserLog);
            Debug.Log("Creo la cartella : "+ pathUserLog);
            _fileDescriptors = new Dictionary<string, FileStream>();
            
            GameObject bluetooth = GameObject.FindGameObjectWithTag("Bluetooth");
            float time = Time.time;
            
            if (bluetooth != null)
            {
                LoggerDevice dev = new LoggerDevice(){
                    DeviceName = "Bluetooth",
                    _transform = bluetooth.transform,
                    localRotation = true,
                };
                _loggerDevices.Add(dev);
            }
            if(_inputDevices.name != null){
                LoggerDevice dev = new LoggerDevice(){
                    inputAction = true,
                    DeviceName = _inputDevices.name,
                    movementFB = _inputDevices.movementFB,
                    movementLR = _inputDevices.movementLR,
                    playerOrientation = _inputDevices.playerOrientation
                };
                _loggerDevices.Add(dev);
            }
            
            // create the log file in the log directory of the project
            
            foreach(var device in _loggerDevices){
                FileStream file = File.Open(pathUserLog+device.DeviceName+".csv", FileMode.OpenOrCreate);
                
                Debug.Log("Creo file: "+pathUserLog+device.DeviceName+".csv");
                
                AddText(file, device.GetHeader(time));
                _fileDescriptors.Add(device.DeviceName,file);
            }
            
            game.loggerManager = this;

        }
    }
    
    private void Start()
    {
        timeSynchronization = GetComponent<TimeSynchronization>();
    }

    private static void AddText(FileStream fs, string value)
    {
        byte[] info = new UTF8Encoding(true).GetBytes(value);
        fs.Write(info, 0, info.Length);
    }

    private void OnDestroy(){
        // save data and close all files
        if (_loggerDevices != null && game.GetActiveLogger()){
            foreach(var l in _loggerDevices)
            {
                foreach(string line in l.GetLines()){
                    AddText(_fileDescriptors[l.DeviceName], line);
                }
                _fileDescriptors[l.DeviceName].Close();
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeSynchronization.IsConnected && !infoSended)
        {
            infoSended = true;
            timeSynchronization.SendInfoMessage(infoSyncMessage);
        }
        try{
            
            if(game.GetActiveLogger()){
                if (batchLogLine < counterLogLine){
                    StartCoroutine(SaveOnFile());
                }
                foreach(var logger in _loggerDevices){
                    logger.SaveLogInBuffer(Time.time, (int)sessionManager.GameStatus);
                }
            }
        }catch(Exception e)
        {
            Debug.Log(e);
            Destroy(this);
        }
        
        
    }

    public void EventLogger(int code)
    {
        try{
            if(game.GetActiveLogger()){
                if (batchLogLine < counterLogLine){
                    StartCoroutine(SaveOnFile());
                }
                foreach(var logger in _loggerDevices){
                    logger.SaveLogInBuffer(Time.time, (int)sessionManager.GameStatus, code);
            }

        }
        }catch(Exception e)
        {
            Debug.Log(e);
            Destroy(this);
        }

    }
    public void SendEvent(string message){
        timeSynchronization.SendSynchronizationMessage(message);
    }

    private IEnumerator SaveOnFile(){
        foreach(var logger in _loggerDevices){
            foreach(var line in logger.GetLines()){
                AddText(_fileDescriptors[logger.DeviceName], line);
                yield return null;
            }
        }
        
    }

}
