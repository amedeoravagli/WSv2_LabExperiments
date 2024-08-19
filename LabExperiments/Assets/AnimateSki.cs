using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimateSki : MonoBehaviour
{
    [SerializeField] private Transform _rotPoint;
    private float _angle = 0;
    private int _rotateDir = 1;
    private float _speedRot = 30.0f;
    private float _angleLimit = 40.0f;
    private bool leftPressed = false, rightPressed = false;
    //private UnityAction<bool> _movingEvent;
    /*private void Start()
    {
        _movingEvent += eventHandler;
    }*/

    /*private void eventHandler(bool isright)
    {
        StartCoroutine()
    }*/

    private void Update()
    {
        Vector3 newEulerOffset = Vector3.zero;
        Quaternion startRotation = transform.localRotation;
        Vector3 startEulerRotation = startRotation.eulerAngles;
        Vector3 finishEulerRotation = new Vector3(0, _angleLimit, 0);
        
        if (Input.GetKey(KeyCode.A))
        {
            _rotateDir = -1;
            leftPressed = true;
            newEulerOffset = Vector3.up * (_speedRot * Time.deltaTime) * _rotateDir;
            if ((startRotation * Quaternion.Euler(newEulerOffset)).eulerAngles.y < -_angleLimit)
            {
                transform.rotation = startRotation * Quaternion.Euler(finishEulerRotation - startEulerRotation);
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            _rotateDir = 1;
            rightPressed = true;
            newEulerOffset = Vector3.up * (_speedRot * Time.deltaTime) * _rotateDir;
            if ((startRotation * Quaternion.Euler(newEulerOffset)).eulerAngles.y > _angleLimit)
            {
                transform.rotation = startRotation * Quaternion.Euler(finishEulerRotation - startEulerRotation);
            }
        }
        else if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)) && startEulerRotation.y != 0)
        {
            _rotateDir = -1 * (int)Mathf.Sign(startEulerRotation.y);
            leftPressed = false;
            rightPressed = false;
            BackRotateSkiUpdate();
        }
        RotateSkiUpdate(newEulerOffset);

        /*
        if (Input.GetKeyDown(KeyCode.A) && !leftPressed)
        {
            _rotateDir = 1;
            leftPressed = true;
            //StopAllCoroutines();
            //StartCoroutine(RotateSki());

        }
        else if (Input.GetKeyDown(KeyCode.D) && !rightPressed)
        {
            _rotateDir = -1;
            rightPressed = true;
            //StopAllCoroutines();
            //StartCoroutine(RotateSki());
        }
        else if (Input.GetKeyUp(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            _rotateDir = -1;
            leftPressed = false;
            //StopAllCoroutines();
            //StartCoroutine(BackRotateSki());
        }
        else if (Input.GetKeyUp(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            _rotateDir = 1;
            rightPressed = false;
            //StopAllCoroutines();
            //StartCoroutine(BackRotateSki());
        }*/

    }

    private void RotateSkiUpdate(Vector3 newEulerOffset)
    {
        Quaternion startRotation = transform.localRotation;
        if (Mathf.Sign(newEulerOffset.y)*newEulerOffset.y > 0)
        {
            transform.rotation = startRotation * Quaternion.Euler(newEulerOffset);
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
