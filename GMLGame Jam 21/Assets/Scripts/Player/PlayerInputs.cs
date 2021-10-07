using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour {

    [SerializeField] private CharacterController2D controller;
    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float resetTime = 0.5f;      // the time the reset button is held for before the level resets
    [SerializeField] public bool shouldLimitKeyPresses = false;

    //private bool hasReleasedRight = false;
    //private bool hasReleasedLeft = false;
    //private bool hasPressedJump = false;


    private float horizontalMove = 0f;
    private float climbUpMove = 0f;
    private float climbDownMove = 0f;
    private bool jump = false;
    private bool checkButtonDownForFixedUpdateInput = true;     // Need to use getbuttondown Input but the input is used in the Fixed update method 
    // and passed into CharacterController2D. This means that it is possible for the keystroke to be detected one Update frame, overidden the next 
    // and then used in fixed update the input will only be checked in th eupdate method if this bool is true. THis bool will then be set to false.
    //  It gets set back to true in the fixed Update method

    private Coroutine resetTimer;
    private bool isResetTimerRoutineRunning = false;

    private int left;
    private int right;
    private int up;
    private int down;

    public void Awake()
    {
        MovementSelection();
        //StartCoroutine(Movement());
    }
    // Update is called once per frame
    void Update() 
    {
        //StartCoroutine(Movement());
        if (left == 1)
        {
            // Get Left Input
            if (controller.HasReleasedLeft == false)
            {
                horizontalMove = Input.GetAxisRaw("HorizontalLeft") * runSpeed;
            }
            else { horizontalMove = 0; }        // stops the hozizontalMove value getting bigger every frame the key is held down
            if (Input.GetButtonUp("HorizontalLeft") && shouldLimitKeyPresses)
            {
                if (!controller.RopeControls.Attached)
                {        // dont count swinging on the rope towards limititing inputs
                    controller.HasReleasedLeft = true;
                    UIManager.Instance.UsedLeft();
                }

            }

            // Get Right Input
            if (controller.HasReleasedRight == false)
            {
                horizontalMove += Input.GetAxisRaw("HorizontalRight") * runSpeed;

            }
            else { horizontalMove += 0; }
            if (Input.GetButtonUp("HorizontalRight") && shouldLimitKeyPresses)
            {
                if (!controller.RopeControls.Attached)
                {        // dont count swinging on the rope towards limititing inputs
                    controller.HasReleasedRight = true;
                    UIManager.Instance.UsedRight();
                }

            }
        }

        else if (left == 2)
        {
            // Get Left Input
            if (controller.HasReleasedLeft == false)
            {
                horizontalMove = Input.GetAxisRaw("HorizontalLeft") * -runSpeed;
            }
            else { horizontalMove = 0; }        // stops the hozizontalMove value getting bigger every frame the key is held down
            if (Input.GetButtonUp("HorizontalLeft") && shouldLimitKeyPresses)
            {
                if (!controller.RopeControls.Attached)
                {        // dont count swinging on the rope towards limititing inputs
                    controller.HasReleasedLeft = true;
                    UIManager.Instance.UsedLeft();
                }

            }

            // Get Right Input
            if (controller.HasReleasedRight == false)
            {
                horizontalMove += Input.GetAxisRaw("HorizontalRight") * -runSpeed;

            }
            else { horizontalMove += 0; }
            if (Input.GetButtonUp("HorizontalRight") && shouldLimitKeyPresses)
            {
                if (!controller.RopeControls.Attached)
                {        // dont count swinging on the rope towards limititing inputs
                    controller.HasReleasedRight = true;
                    UIManager.Instance.UsedRight();
                }

            }
        }
        // Jump Input
        // This version is for the only jump or climb not both version
        if (Input.GetButtonDown("VerticalUp"))
        {
            jump = true;
        }


        //Climb Up
        climbUpMove = Input.GetAxisRaw("VerticalUp");
        // Climb down - Now that climb down is essentially just drop/jump off the vine I only want it too happen if the player presses the key...
        // but dont want to change all the code to use a bool instead
        if (Input.GetButtonDown("VerticalDown") && checkButtonDownForFixedUpdateInput)
        {
            climbDownMove = -1;
            checkButtonDownForFixedUpdateInput = false;

        }
        else if (checkButtonDownForFixedUpdateInput)
        {
            climbDownMove = 0;
        }

        // Reset Input
        if (Input.GetButtonDown("Reset")) {
            GameManager.Instance.ResetLevel();
            //UIManager.Instance.StartResetImage(resetTime);
        }
        if (Input.GetButtonUp("Reset")) {
            //UIManager.Instance.StopResetImage();
        }

    }

    void FixedUpdate() {
        controller.Move(horizontalMove * Time.fixedDeltaTime, climbUpMove, climbDownMove, jump);
        jump = false;
        checkButtonDownForFixedUpdateInput = true;
    }

    private void ResetPlayer() {
        controller.ResetPlayer();
    }
    

    public void IsPlayerStillClimbing(ref bool up, ref bool down) {
        if (Input.GetButtonUp("VerticalUp") && up == false) {           // only need to check if the has still not stopped climbing i.e. up is false
            up = true;
            UIManager.Instance.UsedClimbUp();                           // tell the UI Manager to grey out the climb up icon

        }
        if (Input.GetButtonUp("VerticalDown") && down == false) {
            down = true;
            UIManager.Instance.UsedClimbDown();                         // tell the UI Manager to grey out the climb up icon

        }
    }

    public void IsPlayerStillClimbingUp(ref bool up) {
        if (Input.GetButtonUp("VerticalUp") && up == false) {           // only need to check if the has still not stopped climbing i.e. up is false
            up = true;
            UIManager.Instance.UsedClimbUp();                           // tell the UI Manager to grey out the climb up icon

        }
    }

    public void MovementSelection()
    {
        // 1 -> left;
        // 2 -> right;
        // 3 -> up;
        // 4 -> down;
        //int count = 1;
        left = Random.RandomRange(1, 3);
        if (left == 1)
        {
            right = 2;
        }
        else
        {
            right = 1;
        }
        Debug.Log(left);
        Debug.Log(right);
    }

    //IEnumerator Movement()
    //{
    //    // 1 -> left;
    //    // 2 -> right;
    //    // 3 -> up;
    //    // 4 -> down;
    //    //int count = 1;
    //    //yield return new waitforseconds(20f);
    //    left = Random.RandomRange(1, 3);
    //    if (left == 1)
    //    {
    //        right = 2;
    //    }
    //    else
    //    {
    //        right = 1;
    //    }
    //    yield return new WaitForSeconds(20f);
    //    //left = Random.RandomRange(1, 3);
    //    //if (left == 1)
    //    //{
    //    //    right = 2;
    //    //}
    //    //else
    //    //{
    //    //    right = 1;
    //    //}
    //    //yield return new WaitForSeconds(20f);
    //}
}
