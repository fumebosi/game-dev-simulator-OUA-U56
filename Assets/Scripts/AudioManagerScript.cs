using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    AudioSource AudioSource;
    public AudioClip music;
    float musicTime = 0;
    
    private void Awake()
    {
        //AudioSource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        //StartCoroutine(FadeInMusic());
    }

    IEnumerator FadeInMusic()
    {
        float timer1 = 0.0f;
        
        while (musicTime < 227)
        {
            musicTime += Time.deltaTime;
            timer1 += Time.deltaTime * 0.3f;
            if (timer1 >= 1)
                timer1 = 1f;
            else
            {
                AudioSource.volume = Mathf.SmoothStep(AudioSource.volume, 1f, timer1);
            }
        }
        //StartCoroutine(FadeOutMusic());
        yield return null;
    }

    IEnumerator FadeOutMusic()
    {
        float timer1 = 0.0f;
        while (musicTime >= 227 && musicTime < 237)
        {
            musicTime += Time.deltaTime;
            timer1 += Time.deltaTime * 0.3f;
            if (timer1 >= 1)
                timer1 = 1f;
            else
            {
                AudioSource.volume = Mathf.SmoothStep(AudioSource.volume, 0f, timer1);
            }
        }
        musicTime = 0f;
        StartCoroutine(FadeInMusic());
        yield return null;
    }
  
}
