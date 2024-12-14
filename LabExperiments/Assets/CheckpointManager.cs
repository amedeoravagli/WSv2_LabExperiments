using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private int checkpointID;
    [SerializeField] private SkateGameManager skateGameManager;
    private BoxCollider area;
    void Start(){
        area = GetComponentInParent<BoxCollider>();
    }

    void Update()
    {
        if((checkpointID == 0 || checkpointID == 1) && skateGameManager.GameStatus == GameStatus.PLAYING){
            gameObject.SetActive(false);
        }else if((checkpointID == 0 || checkpointID == 1) && (skateGameManager.GameStatus == GameStatus.TRAIN || skateGameManager.GameStatus == GameStatus.START))
        {
            gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
            switch(checkpointID){
                case 0: //train
                    float xMinInclusive = area.bounds.min.x/2;
                    float xMaxExclusive = area.bounds.max.x/2;
                    float zMinInclusive = area.bounds.min.z/2;
                    float zMaxExclusive = area.bounds.max.z/2;
                    //Debug.Log(area.bounds.min);
                    //Debug.Log(area.bounds.max);
                    transform.localPosition = new Vector3(UnityEngine.Random.Range(xMinInclusive, xMaxExclusive), 0, UnityEngine.Random.Range(zMinInclusive, zMaxExclusive));
                    break;
                case 1: // start
                    skateGameManager.isReadyToStart = true;
                    skateGameManager.ChangeState(GameStatus.START);
                    break;
                case 2: // first
                    skateGameManager.manageDoor.Invoke(1);
                    Destroy(gameObject);
                    break;
                case 3: // second
                    skateGameManager.manageDoor.Invoke(2);
                    Destroy(gameObject);
                    break;
                case 4: // finish
                    skateGameManager.ChangeState(GameStatus.FINISH);
                    Destroy(gameObject);
                    break;
                    
            }
        }
    }

    
    private void OnTriggerExit(Collider other){
        if(other.gameObject.CompareTag("Player")){
            switch(checkpointID){
                case 1: // start
                    skateGameManager.isReadyToStart = false;
                    skateGameManager.ChangeState(GameStatus.TRAIN);
                    break;
                    
            }
        }
    }
}
