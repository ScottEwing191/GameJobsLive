using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footstep : MonoBehaviour
{
    [SerializeField] Transform groundCheckCollider; 
    [SerializeField] LayerMask groundLayer;
    const float groundCheckRadius = 0.2f;
    [SerializeField] bool isGrounded = false; //for debug


    [SerializeField] AudioClip[] audioClip;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        GroundCheck();
    }

    void GroundCheck()
    {
        isGrounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckCollider.position, groundCheckRadius, groundLayer);
        if (colliders.Length > 0)
            isGrounded = true;
    }

    //Event for the Run animation
    private void Step()
    {
        if (isGrounded)
        {
            AudioClip clip = GetRandomClip();
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.volume = Random.Range(0.1f, 0.15f);
            audioSource.PlayOneShot(clip);
        }
    }

    private AudioClip GetRandomClip()
    {
        int index = Random.Range(0, audioClip.Length - 1);
        return audioClip[index];
    }
}
