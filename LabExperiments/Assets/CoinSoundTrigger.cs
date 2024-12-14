using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSoundTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player")){
            //GetComponent<AudioSource>().Play();
            Destroy(gameObject, 0.5f);
        }
    }
}
