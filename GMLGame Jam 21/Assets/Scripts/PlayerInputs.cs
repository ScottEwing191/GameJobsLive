using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour {

    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float resetTime = 0.5f;      // the time the reset button is held for before the level resets
    [SerializeField] public bool shouldLimitKeyPresses = true;

    private bool hasReleasedRight = false;
    private bool hasReleasedLeft = false;
    private bool hasPressedJump = false;

    
    private float horizontalMove = 0f;
    private float climbUpMove = 0f;
    private float climbDownMove = 0f;
    private bool jump = false;

    private Coroutine resetTimer;
    private bool isResetTimerRoutineRunning = false;


    // Update is called once per frame
    void Update() {

        // Get Left Input
        if (controller.HasReleasedLeft == false) {
            horizontalMove = Input.GetAxisRaw("HorizontalLeft") * runSpeed;
        }
        else { horizontalMove = 0; }
        if (Input.GetButtonUp("HorizontalLeft") && shouldLimitKeyPresses) {
            controller.HasReleasedLeft = true;
        }

        // Get Right Input
        if (controller.HasReleasedRight == false) {
            horizontalMove += Input.GetAxisRaw("HorizontalRight") * runSpeed;
        }
        else { horizontalMove += 0; }
        if (Input.GetButtonUp("HorizontalRight") && shouldLimitKeyPresses) {
            controller.HasReleasedRight = true;
        }

        // Jump Input
        //if (Input.GetButtonDown("Jump")) {
        if (Input.GetButtonDown("VerticalUp")) {

            jump = true;
        }

        //Climb Up
        climbUpMove = Input.GetAxisRaw("VerticalUp");
        climbDownMove = Input.GetAxisRaw("VerticalDown");

        // Reset Input
        if (Input.GetButtonDown("Reset")) {
            resetTimer = StartCoroutine(ResetTimer());
        }
        if (Input.GetButtonUp("Reset") && isResetTimerRoutineRunning) {
            StopCoroutine(resetTimer);
        }

    }

    void FixedUpdate() { 
        controller.Move(horizontalMove * Time.fixedDeltaTime, climbUpMove, climbDownMove, jump);
        jump = false;
    }

    IEnumerator ResetTimer() {
        isResetTimerRoutineRunning = true;
        yield return new WaitForSeconds(resetTime);
        ResetPlayer();
        isResetTimerRoutineRunning = false;
    }

    private void ResetPlayer() {
        controller.ResetPlayer();
        //ResetInputs();
    }
    public void ResetInputs() {
        hasReleasedLeft = false;
        hasReleasedRight = false;
        hasPressedJump = false;
    }

    public void IsPlayerStillClimbing(ref bool up, ref bool down) {
        if (Input.GetButtonUp("VerticalUp") && up == false) {           // only need to check if the has still not stopped climbing i.e. up is false
            up = true;
        }
        if (Input.GetButtonUp("VerticalDown") && down == false) {
            down = true;
        }
    }
}
