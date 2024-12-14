using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ChangeBBoxRealTime : MonoBehaviour
{
    [SerializeField] private GameObject mainCamera;
    private CharacterController character;
    private Vector3 center;
    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        center = character.center;
    }

    // Update is called once per frame
    void Update()
    {
        float height = 1.63f;
        float centerY = 1.13f;
            
        if (XRSettings.enabled){
            height = mainCamera.transform.position.y;
            centerY = height / 2;
        }
        center.y = centerY;
        character.center = center;
        character.height = height;
        
    }
}
