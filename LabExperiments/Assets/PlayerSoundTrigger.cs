using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip checkpoint;
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Point")){
            audioSource.clip = coin;
            audioSource.Play();
        }
        else if(other.gameObject.CompareTag("Checkpoint")){
            audioSource.clip = checkpoint;
            audioSource.Play();
        }
    }
}
