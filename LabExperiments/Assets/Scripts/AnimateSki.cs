using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

using UnityEngine.Events;

public class AnimateSki : MonoBehaviour
{
    [SerializeField] private Transform _rotPoint;
    private Player _player;
    //private float _angle = 0;
    private int _rotateDir = 1;
    private float _speedRot = 5.0f;
    private float _angleLimit = 40.0f;
    //private bool leftPressed = false, rightPressed = false;
    private float offset = 0.0f; // [-1.0; 1.0]
    private float _dzOffset = 0.01f;
    private bool _isDesktop = false;
    private float factor = 0.0f;
    private void Start()
    {
        _player = GetComponentInParent<Player>();
        _isDesktop = GameSingleton.Instance._inputType == InputActionType.DESKTOP;
    }

    private void Update()
    {
        //Quaternion startRotation = transform.localRotation;
        //Vector3 startEulerRotation = startRotation.eulerAngles;
        //Vector3 finishEulerRotation = new Vector3(0, _angleLimit, 0);
        
        float hMove = _player.horizontalMove;
        //Debug.Log("HMove = " + hMove);
        if (_player.isMovingLR && hMove != 0)
        {   
            if(_isDesktop)
            {
                offset += Time.deltaTime * _speedRot * Mathf.Sign(hMove);
            }
            else{ 
                // offset += Time.deltaTime * _speedRot  * hMove;
                offset = hMove;
            }
            if(offset > 0 && offset > hMove)
            {
                offset = hMove;
            }
            if(offset < 0 && offset < hMove)
            {
                offset = hMove;
            }

            factor = 1;
            /*
            if (hMove == 1 || hMove == -1)
                offset += Time.deltaTime * _speedRot * Mathf.Sign(hMove);
            else
                offset = hMove;
            */
            /*   
            if (Mathf.Abs(hMove) == 1)
            {
                offset += Time.deltaTime * _speedRot * Mathf.Sign(hMove);
            }
            else
            {
                offset = hMove;
            }
            */
            /*if(_player.verticalMove < 0) // Input.GetKey(KeyCode.S)
            {
                // rotate ski in A position to simulate the stop
                //RotateSkiUpdate(); 
            }*/

        }else if(offset != 0){
            factor -= Time.deltaTime * 2;
            if (factor < 0)
                factor = 0;
            if(offset < -_dzOffset || offset > _dzOffset) // when player is not sliding left or right but ski are not in the vertical position
            {
                _rotateDir = -1 * (int)Mathf.Sign(offset);
                offset += Time.deltaTime * _speedRot * _rotateDir * factor;
                if (Mathf.Sign(offset) == _rotateDir){
                    offset = 0;
                }
            }else{
                offset = 0;
            }
            
        } 
        
        Vector3 newEulerOffset = _angleLimit * offset * Vector3.up;
        
        if (newEulerOffset.y != 0)
        {
            transform.localRotation = Quaternion.Euler(newEulerOffset);
        }

    }
    
}
