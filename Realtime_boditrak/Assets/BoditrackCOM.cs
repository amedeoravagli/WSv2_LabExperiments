using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Text.Json;
using System.Runtime.CompilerServices;
using System.Linq;
using Palmmedia.ReportGenerator.Core.Common;
using System;
using System.Collections.Concurrent;
using System.Security.Authentication.ExtendedProtection;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

/*
{
"device":{ "class":"Boditrak DataPort", "name":"DataPort-83480C", "id":"3E6AA783480C", "address":"127.0.0.1", "model":"wia" },
"sensors":[ { "name":"BT2-3232-200-000218", "columns":32, "rows":32, "width":470, "height":470, "minimum":0, "maximum":200, "units":"mmHg" } ],
"frames":[ { "id":1224, "time":"2024-12-12 14:54:34.899", "readings":[ [ 0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 ] ] } ],
"filters":{ "spot":false, "smooth":false, "noise":false },
"time":"2024-12-12 14:54:34.899",
"frequency":180000,
"calibrated":true,
"firmware":{ "version":"1.02.003" }
}
*/


[Serializable]
public struct BoditrackFrame{
    public int id;
    public string time;
    public float[] readings;
}

[Serializable]
public struct BoditrakData
{
    
    public Dictionary<string,string> device;
    public List<Dictionary<string,string>> sensors;
    public BoditrackFrame[] frames;
    public Dictionary<string,string> filters;
    public string time;
    public int frequency;
    public bool calibrated;
    public string firmware;

}

public class BoditrackCOM : MonoBehaviour
{
    public ConcurrentQueue<float[]> COMQueue = new ConcurrentQueue<float[]>(); 
    private static HttpClient sharedClient = new()
    {
        BaseAddress = new Uri("http://localhost/"),
    };

    async Task GetAsync(HttpClient httpClient)
    {
        using HttpResponseMessage response = await httpClient.GetAsync("api");
        
        response.EnsureSuccessStatusCode();
        
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var jsonLines = jsonResponse.Split("\n");
        string frame = "";
        foreach( var line in jsonLines){
            if (line.Contains("frames")){
                frame = line.Replace("\"frames\":[ ", "");
                frame = frame.Replace("[ [", "[");
                frame = frame.Replace("] ]", "]");
                frame = frame.Replace("],", "");
            }
        }
        var frameObj = JsonUtility.FromJson<BoditrackFrame>(frame);
 

        float mValue = frameObj.readings.Max();
        float[] heatmap = new float[1024];
        if (mValue>0)
        {
            for(int i = 0; i < 1024; i++){
                heatmap[i] = frameObj.readings[i]/mValue;
            }
        }
        COMQueue.Enqueue(heatmap);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    async void Update()
    {
        /*BoditrackFrame frame = */
        await GetAsync(sharedClient);
        
    }

    /* 
    Da fare:
    - Alla chiusura del programma devono essere aspettate le richieste pending
    */
}
