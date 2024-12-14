using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioEngine : MonoBehaviour
{
    [SerializeField] SessionManager sessionManager;
    [SerializeField] AudioMixerGroup mixer;
    [Range(-1f, 1f)]
    public float offset;
    public bool breakIsOn = false;
    public bool speedUpOn = false;
    private float cutoffOn = 4000;
    private float cutoffOff = 8000;
    private MovePlayer player;
    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<MovePlayer>();
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        float freqLowPassCut = cutoffOff;
        //float freqHighPassCut = 10;
        if(sessionManager.GameStatus == GameStatus.PLAYING){
            if (player.isMovingFB)
            {
                float rangefreq = cutoffOff - cutoffOn;
                freqLowPassCut = cutoffOn + (((player.verticalMove+1)/2 + 0.5f) * rangefreq);
                //Mathf.Lerp(cutoffOn, cutoffOff, (player.verticalMove+1) / 2);
                /*if(player.verticalMove <= 0)
                {
                    freqLowPassCut = Mathf.Lerp(cutoffOff, cutoffOn, -player.verticalMove);
                }
                else if(player.verticalMove > 0)
                {
                    freqHighPassCut = Mathf.Lerp(10, highCutOff, player.verticalMove);
                }*/
            }
            if(player.isMovingLR){
                freqLowPassCut -= Mathf.Abs(player.horizontalMove) * 1000;
            }
        }else {
            freqLowPassCut = 10;
        }
        mixer.audioMixer.SetFloat("freqLowPass", freqLowPassCut);
//        mixer.audioMixer.SetFloat("freqHighPass", freqHighPassCut);
//        lowPassFilter.cutoffFrequency = freqLowPassCut;
//        highPassFilter.cutoffFrequency = freqHighPassCut;
        
    }

/*    void OnAudioFilterRead(float[] data, int channels)
    {
        // white noise generation
        for (int i = 0; i < data.Length; i++)
        {
            float d = (float)(rand.NextDouble() * 2.0 - 1.0 + offset);
            for (int j = 0; j < channels; j++)
            {
                data[i+j] = d;
            }
        }
    }
*/


}
