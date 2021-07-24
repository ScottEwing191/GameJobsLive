using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicRandomPosStart : MonoBehaviour
{
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.time = Random.Range(0f, audioSource.clip.length);
        audioSource.Play();
    }
}
