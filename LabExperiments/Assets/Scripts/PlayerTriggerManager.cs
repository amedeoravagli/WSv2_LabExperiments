using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTriggerManager : MonoBehaviour
{
    public GameObject _newSubTrack;
    public Transform _track;
    private GameObject _GOSubTrack;
 
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("SubTrack"))
        {
            _GOSubTrack = Instantiate(_newSubTrack);
            _GOSubTrack.transform.parent = _track;
            Vector3 pos = _GOSubTrack.transform.localPosition;
            _GOSubTrack.transform.localPosition = new Vector3(pos.x, 0, pos.z); 
        }
    }
    
}
