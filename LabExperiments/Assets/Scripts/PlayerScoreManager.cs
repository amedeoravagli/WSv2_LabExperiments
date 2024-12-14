
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    
    //[SerializeField] private GameObject _postProcessFeedback;
    //[SerializeField] private AudioClip _hitObstacle;
    //[SerializeField] private AudioClip _hitCoin;
    public int _score = 0;
    public TMPro.TMP_Text _scoreUI;

    private Animator _visualFeedBack;
    private AudioSource _audioSource;
    //private string _damegeColor = "BE2E2E";
   

    private void Start(){
        //Assert.IsNotNull(_scoreUI, "ScoreUI reference is null");
        _scoreUI.text = _score.ToString();
        //_visualFeedBack = _postProcessFeedback.GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        //_visualFeedBack.Stop();
        
    }

    void Update(){
        _scoreUI.text = _score.ToString();
    }
}
