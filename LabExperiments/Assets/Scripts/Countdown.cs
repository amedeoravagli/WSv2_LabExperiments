using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Countdown : MonoBehaviour
{
    private TMP_Text _tmpText;
    private string text = "3";
    public float _countdown = 3;
    private bool _isCounting = false;
    void Start()
    {
        _tmpText = GetComponentInChildren<TMP_Text>();
        _tmpText.color = Color.black;
        _tmpText.text = "Calibration.. Press a button when you are ready";
        _isCounting = false;
    }

    // Update is called once per frame
    void Update()
    {   
        if(_isCounting){
            if (_countdown > 0)
            {
                _countdown -= Time.deltaTime;
                text = ((int)_countdown+1).ToString();
                _tmpText.text = text;
            }else{
                
                _tmpText.text = "";
                _isCounting = false;
                //transform.gameObject.SetActive(false);
            }
        }
        
        
    }

    void OnEnable()
    {
        _tmpText = GetComponentInChildren<TMP_Text>();
        _tmpText.text = "Calibration.. Press a button when you are ready";
        _isCounting = false;
        _countdown = 3;
    }

    public void startCounting(){
        _isCounting = true;
    }
}
