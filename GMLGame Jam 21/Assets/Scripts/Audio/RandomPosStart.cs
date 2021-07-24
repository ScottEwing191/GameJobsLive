using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosStart : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.time = Random.Range(0f, audioSource.clip.length);
        audioSource.pitch = Random.Range(1.8f, 2.8f);
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
