using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimateSki : MonoBehaviour
{
    [SerializeField] private Transform _rotPoint;
    [SerializeField] private Transform _backRotPoint;
    //private float _angle = 0;
    private int _rotateDir = 1;
    private float _speedRot = 5.0f;
    private float _factor = 5.0f;
    private float _angleLimit = 40.0f;
    //private bool leftPressed = false, rightPressed = false;
    private float offset = 0.0f; // [-1.0; 1.0]
    private float _dzOffset = 0.05f;
    

    private void Update()
    {
        Vector3 newEulerOffset = Vector3.zero;
        //Quaternion startRotation = transform.localRotation;
        //Vector3 startEulerRotation = startRotation.eulerAngles;
        //Vector3 finishEulerRotation = new Vector3(0, _angleLimit, 0);

        
        if (Input.GetKey(KeyCode.A) && offset > -1.0)
        {
            _rotateDir = -1;
            //leftPressed = true;
            offset += Time.deltaTime * _factor * _rotateDir;
            if(offset < -1.0){
                offset = -1.0f;
            }
        }
        else if (Input.GetKey(KeyCode.D) && offset < 1.0)
        {
            _rotateDir = 1;
            //rightPressed = true;
            offset += Time.deltaTime * _factor * _rotateDir;
            if(offset > 1.0){
                offset = 1.0f;
            }
        }
        else if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) && (offset < -_dzOffset || offset > _dzOffset))
        {
            _rotateDir = -1 * (int)Mathf.Sign(offset);
            //leftPressed = false;
            //rightPressed = false;
            offset += Time.deltaTime * _factor * _rotateDir;
        }
        newEulerOffset = _angleLimit * offset * Vector3.up;
        
        if(Input.GetKey(KeyCode.S))
        {
            // rotate ski in A position to simulate the stop
            //RotateSkiUpdate(); 
        }
        RotateSkiUpdate(newEulerOffset);

    }

    private void RotateSkiUpdate(Vector3 newEulerOffset)
    {
        
        if (newEulerOffset.y != 0)
        {
            transform.localRotation = Quaternion.Euler(newEulerOffset);
        }
    }
    private void BackRotateSkiUpdate()
    {
        Quaternion startRotation = transform.localRotation;
        Vector3 newEulerOffset = Vector3.up * (_speedRot * Time.deltaTime) * _rotateDir;
        if ((startRotation * Quaternion.Euler(newEulerOffset)).eulerAngles.y < 0)
        {
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            transform.rotation = startRotation * Quaternion.Euler(newEulerOffset);
        }
    }


    IEnumerator RotateSki()
    {

        Quaternion startRotation = transform.localRotation;
        float endZRot = startRotation.eulerAngles.y;
        float duration = 1f;
        float t = 0;
        
        while (t < 1f)
        {
            t = Mathf.Min(1f, t + Time.deltaTime / duration);
            Vector3 newEulerOffset = Vector3.up * (endZRot * t) * _rotateDir;
            // global z rotation
            //transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            // local z rotation
            transform.rotation = startRotation * Quaternion.Euler(newEulerOffset);
            //transform.RotateAround(_rotPoint.position, Vector3.up, (endZRot * t) * _rotateDir);
            yield return null;
        }
        /*for (float angle = _angle; _angle < 45; _angle += 0.1f)
        {
            transform.RotateAround(_rotPoint.position, Vector3.up, _angle * _rotateDir);
            //_angle = angle;
            yield return null;

        }*/
    }
    IEnumerator BackRotateSki()
    {
        Quaternion startRotation = transform.rotation;
        float endZRot = 40.5f;
        float duration = 1f;
        float t = 1;
        
        while (t > 0f)
        {
            t = Mathf.Max(0f, t - Time.deltaTime / duration);
            Vector3 newEulerOffset = Vector3.up * (endZRot * t) * _rotateDir;
            // global z rotation
            //transform.rotation = Quaternion.Euler(newEulerOffset) * startRotation;
            // local z rotation
            transform.rotation = startRotation * Quaternion.Euler(newEulerOffset);
           // transform.RotateAround(_rotPoint.position, Vector3.up, (endZRot * t) * _rotateDir);
            yield return null;
        }
        /*for (float angle = _angle; _angle > 0 ; _angle -= 0.1f)
        {
            transform.RotateAround(_rotPoint.position, Vector3.up, _angle * _rotateDir);
            //_angle = angle;
            yield return null;
        }*/

    }
}
