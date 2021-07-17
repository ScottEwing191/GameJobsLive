using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    
    [SerializeField] private AudioSource climbingSource;
    [SerializeField] private AudioSource fireDeathSource;
    [SerializeField] private AudioSource jumpSource;
    private Animator anim;
    private CharacterController2D controller;           // To check if the player is grounded

    // === Properties ===

    public AudioSource ClimbingSource {
        get { return climbingSource; }
        set { climbingSource = value; }
    }


    private void Start() {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    private void Climb() {
        if (!climbingSource.isPlaying) {
            climbingSource.time = Random.Range(0f, climbingSource.clip.length);
            climbingSource.volume = 0.6f;
            climbingSource.Play();
        }
    }

    private void IdleClimb() {
        if (climbingSource.isPlaying) {
            climbingSource.Stop();
        }
    }

    private void FireDeath() {
        climbingSource.Stop();
        if (!fireDeathSource.isPlaying) {
            fireDeathSource.Play();
            fireDeathSource.volume = 0.3f;

        }
    }

    private void JumpSound() {
        if (!jumpSource.isPlaying) {
            jumpSource.Play();
            jumpSource.volume = 0.2f;
        }
    }

}
