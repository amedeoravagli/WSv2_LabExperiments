using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.OpenXR.Input;

public class ObjectCollisionManager : MonoBehaviour
{
    private LoggerManager logger;
    [SerializeField] private int points;
    private GameObject _visualFeedBack;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private InputActionReference lefthapticAction;
    [SerializeField] private InputActionReference righthapticAction;
    float _amplitude = 1.0f;
    float _duration = 0.5f;
    float _frequency = 0.0f;
    void Start()
    {
        GameObject loggerGO = GameObject.FindGameObjectWithTag("Logger");
        if (loggerGO != null){
            logger = loggerGO.GetComponent<LoggerManager>();
        }
        _visualFeedBack = GameObject.FindGameObjectWithTag("FeedBack");
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerScoreManager>()._score += points;
            // animate hit feedback visual and audio
            if (points < 0)
            {
                if(logger != null){
                    logger.EventLogger(3);
                }
                _visualFeedBack.GetComponent<Animator>().SetTrigger("hit");
                if (lefthapticAction != null)
                {
                    OpenXRInput.SendHapticImpulse(lefthapticAction, _amplitude, _frequency, _duration);
                }
                if (righthapticAction != null)
                {
                    OpenXRInput.SendHapticImpulse(righthapticAction, _amplitude, _frequency, _duration);
                }
            }    
            other.gameObject.GetComponent<AudioSource>().clip = audioClip;
            other.gameObject.GetComponent<AudioSource>().Play();
            if(gameObject.CompareTag("Point")){
                if(logger != null) {
                    logger.EventLogger(4);
                    
                }
                Destroy(transform.parent.gameObject, 0.1f);
            }
            
        }
        
    }
}
