using System.Collections.Generic;
using UnityEngine;
using System.IO;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;
using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine.InputSystem;



public class BTConnectionJob : MonoBehaviour, IBTConnection
{
    [SerializeField] private string _btNameDevice = "WalkingSeatBT";
    [SerializeField] private InputActionReference calibrateOrientation;
    [SerializeField] private InputActionReference headOrientation;
    private BluetoothClient client;
    private BluetoothClient localClient; 
    //private Stream stream;
    private Stream localStream;
    private bool stopConnection = false;

    private float rx = 0, ry = 0, rz = 0, rw = 0;
    private float angleRotX = 0.0f, angleRotY = 0.0f, angleRotZ = 0.0f, angleRotW = 0.0f;
    private Vector3 offsetPose = Vector3.zero;
    private bool firstPose = false;
    private object _lock = new();
    private object _lockStopConnection = new();
    private bool _isConnected = false;


    public bool IsConnected(){
        bool result = false;
        lock(_lockStopConnection)
        {
            result = _isConnected;
        }
        return result; 
    }
    
    void OnDestroy()
    {
        lock(_lockStopConnection)
        {
            stopConnection = true;
            _isConnected = false;
        }
        transform.localEulerAngles = Vector3.zero;   
        
        
    }

    void OnDisable()
    {
        lock(_lockStopConnection)
        {
            stopConnection = true;
            _isConnected = false;
        }
        transform.localEulerAngles = Vector3.zero;   
    }

    void Awake(){
        GameSingleton game = GameSingleton.Instance;
        calibrateOrientation.action.performed += CalibrateSeatOrientation;
        if(game.bluetooth != null){
            Destroy(this);
        }
        else
        {
            game.bluetooth = this;
        }
    }

    async void OnEnable()
    {
        try
        {
            firstPose = false;
            client = new BluetoothClient
            {
                Authenticate = false
            };
            stopConnection = false;


            Debug.Log("Ready to call async function..");
            await Task.Run(() => ListenWSThread());

        }catch(Exception e){
            Debug.Log(e);
            lock(_lockStopConnection)
            {
                stopConnection = true;
                client.Dispose();
            }
        }
        
    }

    void FixedUpdate()
    {
        bool localIsConnected = false;
        lock(_lockStopConnection)
        {
            localIsConnected = _isConnected;
            localClient = client;
        }
        if(localIsConnected && localClient != null)
        {
            lock(_lock)
            {
                angleRotX = rx;
                angleRotY = rz;
                angleRotZ = ry;

                if (!firstPose && IsConnected() && angleRotX != 0 && angleRotY != 0 && angleRotZ != 0){
                    offsetPose = new Vector3(angleRotX, angleRotY, angleRotZ);
    
                    Debug.Log(offsetPose);
                    firstPose = true;
                }
            }
            
            //transform.Rotate(new(angleRotX, -angleRotY, angleRotZ));
            transform.localEulerAngles = new Vector3(angleRotX, angleRotY, angleRotZ) - offsetPose;
        }
        
    }

    public void CalibrateSeatOrientation(InputAction.CallbackContext context){
        float yHeadOrientation = headOrientation.action.ReadValue<Quaternion>().eulerAngles.y;
        offsetPose.y = yHeadOrientation;
    }

    private void ListenWSThread()
    {
        BluetoothDeviceInfo deviceInfo = null;
        BluetoothClient localClient = null;
        bool localStopConnection = false;
        lock(_lockStopConnection)
        {
            localClient = client;
            localStopConnection = stopConnection;
        }
        // research bluetooth device
        while(localClient is not null && deviceInfo is null && !localStopConnection)
        {
            Debug.Log("Running the Task..");
            IReadOnlyCollection<BluetoothDeviceInfo> btDevices = localClient.DiscoverDevices(10);
            if (btDevices is null){
                Debug.Log("Bluetooth device list is null");
            }
            Debug.Log("Bluetooth device list count: " + btDevices.Count);
            foreach(BluetoothDeviceInfo d in btDevices)
            {
                Debug.Log(d.DeviceName + " " + d.DeviceAddress);
                if(d.DeviceName == _btNameDevice)
                {
                    deviceInfo = d;
                    break;
                }
            }
            lock(_lockStopConnection)
            {
                localClient = client;
                localStopConnection = stopConnection;
            }
        }
        
        if (localClient != null && !localStopConnection && deviceInfo != null)
        {
            Debug.Log("Try to connect to device: name = " + deviceInfo.DeviceName + " address = "+deviceInfo.DeviceAddress);
            localClient.Connect(deviceInfo.DeviceAddress, BluetoothService.SerialPort);
            Debug.Log("Connection established");
            Stream localStream = localClient.GetStream();
            localStream.Flush();
            Debug.Log(localStream.Length);

            int len = 12;
            byte[] buffer = new byte[len];
            float rX, rY, rZ/*, rW*/;
            try
            {
                while(localStream != null && !localStopConnection)
                {
                    lock(_lockStopConnection){
                        _isConnected = localClient.Connected;
                    }
                
                    while(!localStopConnection && localStream.Read(buffer, 0, buffer.Length) == len)
                    {
                        
                        if(localStream.Length > 1)
                            Debug.Log(localStream.Length);
                        //Thread.Sleep(500);
                        lock(_lockStopConnection){
                            localStopConnection = stopConnection;
                            _isConnected = true;
                        }
                        if (localStopConnection){
                            break;
                        }
                        //Debug.Log("Is reading 12 bytes correctly");                
                        var rotX = new Byte[4];
                        var rotY = new Byte[4];
                        var rotZ = new Byte[4];
                        var rotW = new Byte[4];

                        Array.Copy(buffer, 0, rotX, 0, 4);
                        Array.Copy(buffer, 4, rotY, 0, 4);
                        Array.Copy(buffer, 8, rotZ, 0, 4);

                        rX = BitConverter.ToSingle(rotX, 0);
                        rY = BitConverter.ToSingle(rotY, 0);
                        rZ = BitConverter.ToSingle(rotZ, 0);
                        if (!float.IsNaN(rX) && !float.IsNaN(rY) && !float.IsNaN(rZ)){
                            lock(_lock)
                            {
                                rx = rX;
                                ry = rY;
                                rz = rZ;
                            }
                        }

                    }
                    
                }
            }
            catch(Exception e)
            {
                Debug.Log(e);
                lock(_lockStopConnection)
                {
                    client = null;
                    _isConnected = false;
                    stopConnection = true;
                }
            }
            
        }
            
        lock(_lockStopConnection)
        {
            client.Dispose();
            _isConnected = false;
            stopConnection = true;
        }
        Debug.Log("Bluetooth thread is dead.");

    }

}

