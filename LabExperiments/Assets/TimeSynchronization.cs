using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using LiteNetLib;
using LiteNetLib.Utils;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class TimeSynchronization : MonoBehaviour
{
    NetManager client;
    NetPeer serverDestination;
    NetDataWriter writer;
    [SerializeField] private string serverIpAddress;
    [SerializeField] private int serverPort;
    [SerializeField] private TMP_Text uiConnState;

    private bool isUp = false;

    public bool IsConnected{
        get{
            if(serverDestination != null){
               // Debug.Log(serverDestination.ConnectionState);
                if (serverDestination.ConnectionState == ConnectionState.Connected)
                    return true;
            }
            return false;
        }
    }
    void Awake()
    {
        EventBasedNetListener listener = new EventBasedNetListener();

        writer = new NetDataWriter();
        client = new NetManager(listener);
        int port = 10000; 
        isUp = client.Start("130.192.163.233", "fe80::c7bd:6b1a:1897:125d%23", port);
        while (!isUp){
            port++;
            isUp = client.Start("130.192.163.233", "fe80::c7bd:6b1a:1897:125d%23", port);
        }
        
        Debug.Log("client is ready? " + isUp + " clientport: " + client.LocalPort);
        int hour = DateTime.Now.Hour;
        int mins = DateTime.Now.Minute;
        int secs = DateTime.Now.Second;
        int millis = DateTime.Now.Millisecond;
        //int RTT = 0;
        if (isUp){
            serverDestination = client.Connect(serverIpAddress /* host ip or name */, serverPort /* port */, "connection_key" /* text key or NetDataWriter */);
            Debug.Log("Connessione richiesta: " + serverDestination);
            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod, channel) =>
            {
                writer = new NetDataWriter();
                string message = dataReader.GetString(100);
                Debug.Log("Messaggio dal server : " + message);
                if(message.Contains("RttUpdate"))
                {
                    writer.Put("RttResponse");
                    fromPeer.Send(writer, DeliveryMethod.ReliableOrdered);
                }
                
                //float delay = fromPeer.RoundTripTime / 2;
                //networkDelay.Add(delay);
                
                dataReader.Recycle();
                
            };
        
        }
        
    }

    void Update()
    {
        if (isUp){
            //Debug.Log(IsConnected);
            switch (serverDestination.ConnectionState)
            {
                case ConnectionState.Connected:
                    uiConnState.text = "Pressure Mat State: Connected";
                    //Debug.Log("Connected");
                    break;
                case ConnectionState.Disconnected:
                    uiConnState.text = "Pressure Mat State: Disconnected";
                    serverDestination = client.Connect(serverIpAddress /* host ip or name */, serverPort /* port */, "connection_key" /* text key or NetDataWriter */);
                    break;
                case ConnectionState.Outgoing:
                    uiConnState.text = "Pressure Mat State: Connecting";
                    break;
                case ConnectionState.ShutdownRequested:
                    uiConnState.text = "Pressure Mat State: Restart Connection";
                    //serverDestination = client.Connect(serverIpAddress /* host ip or name */, serverPort /* port */, "connection_key" /* text key or NetDataWriter */);
                    break;
            }
            client.PollEvents();
        }
    }



    void OnDestroy(){
        if (client != null)
            client.Stop();
    }
    // Start is called before the first frame update


    public void SendSynchronizationMessage(string timeMessage)
    {
        if(IsConnected)
        {
            Debug.Log(timeMessage);
            writer = new NetDataWriter();
            writer.Put(timeMessage);
            try{
                if(IsConnected){
                    serverDestination = client.FirstPeer;
                    serverDestination.Send(writer, DeliveryMethod.ReliableOrdered);
                }

            }catch(Exception e)
            {
                Debug.Log(e);
            }
        }
        
    }

    public void SendInfoMessage(string info)
    {
        if(IsConnected){
            Debug.Log("Info:" + info);
            writer = new NetDataWriter();
            writer.Put("Info:"+info);
            try{
                if(IsConnected){
                    serverDestination = client.FirstPeer;
                    serverDestination.Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }catch(Exception e){
                Debug.Log(e);
            }
        }
        
    }

}
