using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerScoreManager : MonoBehaviour
{
    
    [SerializeField] private GameObject _postProcessFeedback;
    [SerializeField] private AudioClip _hitObstacle;
    [SerializeField] private AudioClip _hitCoin;
    public int _score = 0;
    public TMPro.TMP_Text _scoreUI;

    private Animator _visualFeedBack;
    private AudioSource _audioSource;
    //private string _damegeColor = "BE2E2E";
   

    private void Start(){
        Assert.IsNotNull(_scoreUI, "ScoreUI reference is null");
        _scoreUI.text = _score.ToString();
        _visualFeedBack = _postProcessFeedback.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        //_visualFeedBack.Stop();
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            _score -= 10;
            // animate hit feedback visual and audio
            _visualFeedBack.SetTrigger("hit");
            _audioSource.clip = _hitObstacle;
            _audioSource.Play();
        }
        if (other.gameObject.CompareTag("Point"))
        {
            _score += 2;
            _audioSource.clip = _hitCoin;
            _audioSource.Play();
            Destroy(other.gameObject, 0.1f);
            // animate point feedback visual and audio

        }

        _scoreUI.text = _score.ToString();
    }
}
