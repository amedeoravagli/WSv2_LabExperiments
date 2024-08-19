using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubTrackTrigger : MonoBehaviour
{
    public GameObject _newSubTrack;
    public Transform _track;
    private GameObject _GOSubTrack;
 

    private void OnTriggerEnter(Collider other)
    {
        
        if(other.gameObject.CompareTag("Trigger"))
        {
           
            _GOSubTrack = Instantiate(_newSubTrack);
            _GOSubTrack.transform.parent = _track;
            Vector3 pos = _GOSubTrack.transform.localPosition;
            _GOSubTrack.transform.localPosition = new Vector3(pos.x, 0, pos.z); 
        }
    }
}
