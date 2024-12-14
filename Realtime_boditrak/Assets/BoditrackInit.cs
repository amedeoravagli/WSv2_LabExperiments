using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;

public class BoditrackInit : MonoBehaviour
{
    [SerializeField] private GameObject cube;
    [SerializeField] private float offsetCube;
    [SerializeField] private Color lowColor;
    [SerializeField] private Color midColor;
    [SerializeField] private Color highColor;
    private ConcurrentQueue<float[]> COMQueue;
    private float[] heatmap = new float[1024];
    [SerializeField] private GameObject copSphere;
    // Start is called before the first frame update
    void Start()
    {
        COMQueue = GetComponent<BoditrackCOM>().COMQueue;
        for (int i = 0; i < 32; i++){
            for(int j = 0; j < 32; j++){
                heatmap[(i*32)+j] = 0;
                
                //newCube.GetComponent<Renderer>().material.color = Color.Lerp(Color.blue, Color.red, heatmap[i]);
                
                GameObject newCube = Instantiate(cube, transform);
                newCube.transform.localPosition = new Vector3(offsetCube*j, 0, -offsetCube*i);
                newCube.transform.localScale = new Vector3(1.5f, 1.0f, 1.5f);
                newCube.GetComponent<Renderer>().material.color = lowColor;
            }
        }    
    }

    private Vector2 calculateCOP(float[] weights)
    {  
        // return cop normalized from -1 to 1
        Vector2 cop = Vector2.zero;
        float[] sumX = new float[32], sumY = new float[32];

        float copX = 0, copY = 0;
        float sumWeights = 0; 

        for (int i = 0; i < transform.childCount; i++){
            // calculate sum value on x and y axis
            // horizontal axes
            sumX[i%32] += weights[i];
            // vertical axes
            sumY[i/32] += weights[i];
            
            sumWeights += weights[i]; 
        }
        if (sumWeights > 0){
            for(int i = 0; i < 32; i++)
            {
                copX += sumX[i] * i;
                copY += sumY[i] * i; 
            }
            copX /= sumWeights;
            copY /= sumWeights;
            // normalize cop position between -1 and 1
            cop.x = (copX / 16) - 1.0f; 
            cop.y = (copY / 16) - 1.0f;
        }

        return cop;
    }

    // Update is called once per frame
    void Update()
    {
        if(COMQueue.TryDequeue(out heatmap)){
            
            for (int i = 0; i < transform.childCount; i++){
                
                // change each cube color depending on matrix value
                if(heatmap[i] < 0.5){
                    transform.GetChild(i).GetComponent<Renderer>().material.color = Color.Lerp(lowColor, midColor, 2 * heatmap[i]);
                }else{
                    transform.GetChild(i).GetComponent<Renderer>().material.color = Color.Lerp(midColor, highColor, 2 * (heatmap[i] - 0.5f));
                }
                transform.GetChild(i).transform.localScale = new Vector3(1.5f, 1.0f + (3.0f*heatmap[i]), 1.5f);
                

            }
            Vector2 cop = calculateCOP(heatmap); 
            copSphere.transform.localPosition = new Vector3(cop.x*(16*offsetCube), 4, -(16*offsetCube)-(16*cop.y*offsetCube));

        }
    }
}
