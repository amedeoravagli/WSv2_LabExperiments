using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    [SerializeField] private GameObject track; 
    private LoggerManager logger;

    void Start()
    {
        GameObject loggerGO = GameObject.FindGameObjectWithTag("Logger");
        logger = loggerGO.GetComponent<LoggerManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("SubTrack")){
            track.GetComponent<GenerateTrack>().GenerateSubTrack(other.gameObject.transform);
            if(logger != null){
                logger.EventLogger(2);
            }
        }

    }
}
