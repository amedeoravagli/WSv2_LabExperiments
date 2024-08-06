using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTrackTrigger : MonoBehaviour
{
    public GameObject _newSubTrack;
    public Transform _spawnSubTrack;
    
 

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger Entered");
        if(other.gameObject.CompareTag("Trigger"))
        {
            Debug.Log("Reposition SubTrack"); 
            Instantiate(_newSubTrack);
        }
    }
}
