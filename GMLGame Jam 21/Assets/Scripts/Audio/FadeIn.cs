using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class FadeIn : MonoBehaviour
{
    [SerializeField]
    private int fadeInTime = 3;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    private void Update()
    {
        if (audioSource.volume < 1)
        {
            audioSource.volume = audioSource.volume + (Time.deltaTime / (fadeInTime + 1));
        }
        else
        {
            Destroy(this);
        }
    }
}
