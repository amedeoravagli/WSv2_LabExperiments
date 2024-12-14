using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private int doorID;
    [SerializeField] private float rotationClose;
    [SerializeField] private float rotationOpen;
    [SerializeField] private SkateGameManager SkateGameManager;
    private Vector3 startAngle;
    // Start is called before the first frame update
    void Start()
    {
        SkateGameManager.manageDoor += ManageDoor;
        startAngle = transform.localEulerAngles;
    }

    void ManageDoor(int i)
    {
        if(doorID == i){
            GetComponent<AudioSource>().Play();
            StartCoroutine(MoveDoor());
        }
    }

    private IEnumerator MoveDoor()
    {
        for(float rotState = 0.0f; rotState >= 0 && rotState < 1.0f; rotState += 0.01f){
            float yRot = Mathf.Lerp(rotationClose, rotationOpen, rotState);
            startAngle.y = yRot;
            transform.localEulerAngles = startAngle;
            yield return new WaitForSeconds(0.02f);
        }
        (rotationOpen, rotationClose) = (rotationClose, rotationOpen);
    }
}
