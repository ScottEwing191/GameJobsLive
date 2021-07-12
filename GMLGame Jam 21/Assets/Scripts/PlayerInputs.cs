using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{

    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float resetTime = 1f;      // the time the reset button is held for before the level resets
    [SerializeField] bool shouldLimitKeyPresses = false;
    
    private bool hasReleasedRight = false;
    private bool hasReleasedLeft = false;
    private bool hasPressedJump = false;


    private float horizontalMove = 0f;
    private bool jump = false;

    private Coroutine resetTimer;
    private bool isResetTimerRoutineRunning = false;
   

    // Update is called once per frame
    void Update()
    {

        // Get Left Input
        if (hasReleasedLeft == false) {
            horizontalMove = Input.GetAxisRaw("HorizontalLeft") * runSpeed; 
        }
        else { horizontalMove = 0;}
        if (Input.GetButtonUp("HorizontalLeft") && shouldLimitKeyPresses) {
            hasReleasedLeft = true;
        }

        // Get Right Input
        if (hasReleasedRight == false) {
            horizontalMove += Input.GetAxisRaw("HorizontalRight") * runSpeed;
        }
        else { horizontalMove += 0; }
        if (Input.GetButtonUp("HorizontalRight") && shouldLimitKeyPresses) {
            hasReleasedRight = true;
        }
        
        // Jump Input
        if (Input.GetButtonDown("Jump") && !hasPressedJump && shouldLimitKeyPresses)
        {
            jump = true;
            hasPressedJump = true;
        }
        // Reset Input
        if (Input.GetButtonDown("Reset")) {
            resetTimer = StartCoroutine(ResetTimer());
        }
        if (Input.GetButtonUp("Reset") && isResetTimerRoutineRunning) {
            StopCoroutine(resetTimer);
        }

    }

    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime,false, jump);
        jump = false;
    }

    IEnumerator ResetTimer() {
        isResetTimerRoutineRunning = true;
        yield return new WaitForSeconds(resetTime);
        ResetPlayer();
        isResetTimerRoutineRunning = false;
    }

    private void ResetPlayer() {
        controller.ResetPosition();
        ResetInputs();
    }
    public void ResetInputs() {
        hasReleasedLeft = false;
        hasReleasedRight = false;
        hasPressedJump = false;
    }
}
