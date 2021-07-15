using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {
    
    [SerializeField] private AudioSource climbingSource;
    [SerializeField] private AudioSource fireDeathSource;
    [SerializeField] private AudioSource jumpSource;
    private Animator anim;
    private CharacterController2D controller;           // To check if the player is grounded

    private void Start() {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController2D>();
    }

    private void Climb() {
        // NOT WORKING SOUND DOEST STOP WHEN THE ANIMATION DOES. aLSO SOUND DOESNT PLAY WHEN GOING DOWN
        if (anim.speed != 0) {
            if (!climbingSource.isPlaying) {
                climbingSource.volume = 0.5f;
                climbingSource.loop = true;
                climbingSource.Play();

            }
        }
        else {
            climbingSource.Pause();
        }
    }

    private void FireDeath() {
        climbingSource.Stop();
        if (!fireDeathSource.isPlaying) {
            fireDeathSource.Play();
            fireDeathSource.volume = 0.3f;

        }
    }

    private void Jump() {
        if (!jumpSource.isPlaying) {
            jumpSource.Play();
            jumpSource.volume = 0.2f;
        }
    }

}
