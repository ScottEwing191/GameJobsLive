using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class CharacterController2D : MonoBehaviour {
    [SerializeField] private float jumpForce = 400f;                            // Amount of force added when the player jumps.
    [Range(0.01f, 1)] [SerializeField] private float vineJumpMultiplier = 1;    // Modifies the jump force the player gets when jumping from a vine
    [SerializeField] private bool canJumpFromVine = false;

    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;    // How much to smooth out the movement
    [SerializeField] private float climbSpeed = 100f;                               // How fast can the player climb
    [SerializeField] private bool airControl = false;                           // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround;                          // A mask determining what is ground to the character

    [SerializeField] private Transform groundCheck;                         // A position marking where to check if the player is grounded.
    [SerializeField] private bool autoResetLevel = false;                   // should the player automatically reset once the player has used all their actions

    private float resetTime = 2;                // The time after the player pressed the last key before the game resets
    const float groundedRadius = .2f;           // Radius of the overlap circle to determine if grounded

    public bool isGrounded;                 // Whether or not the player is grounded.
    private bool isFacingRight = true;          // For determining which way the player is currently facing.  
    private bool isTouchingClimable = false;    // is the player touching a surface they can climb up
    private bool isClimbing = false;            // CURRENTLY NOT USED FOR ANYTHING  has the player actually started climbing
    private bool isDead = false;                // true once the OnPlayerDeath method is called. Used to stop it activating twice due to multiple colliders on player

    //Movement Tracking Variables
    private bool hasReleasedRight = false;
    private bool hasReleasedLeft = false;
    private bool hasClimbedUp = false;           // Has the player already climbed and stopped climbing i.e. pressed and released the climb buttons (while able to climb)
    private bool hasClimbedDown = false;
    private bool hasJumped;

    private Vector3 startPosition;              // The position that the player will return to when the player Resets
    private Rigidbody2D rb2D;
    private PlayerInputs playerInputs;
    private Animator anim;
    private RopeControls ropeControls;
    private PlayerSounds playerSounds;
    private GameObject canvas;                  // Need to flip the canvas every time the player flipped when the direction changes



    // === PROPERTIES ===

    public bool HasReleasedRight {
        get { return hasReleasedRight; }
        set { hasReleasedRight = value; }
    }

    public bool HasReleasedLeft {
        get { return hasReleasedLeft; }
        set { hasReleasedLeft = value; }
    }

    public bool HasClimbedUp {
        get { return hasClimbedUp; }
        set { hasClimbedUp = value; }
    }
    public bool HasClimbedDown {
        get { return hasClimbedDown; }
        set { hasClimbedDown = value; }
    }
    public bool HasJumped {
        get { return hasJumped; }
        set { hasJumped = value; }
    }
    public bool IsGrounded {
        get { return isGrounded; }
        set { isGrounded = value; }
    }

    public bool IsTouchingClimable {
        get { return isTouchingClimable; }
        set { isTouchingClimable = value; }
    }

    public RopeControls RopeControls {
        get { return ropeControls; }
        set { ropeControls = value; }
    }

    // === PROPERTIES END

    private void Awake() {
        rb2D = GetComponent<Rigidbody2D>();
        playerInputs = GetComponent<PlayerInputs>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;
        ropeControls = GetComponentInChildren<RopeControls>();
        playerSounds = GetComponent<PlayerSounds>();
        canvas = GetComponentInChildren<Canvas>().gameObject;

    }

    private void Update() {
        // If the player is touching the ladder check every frame if the player has released the climb up or down buttons. 
        if (isTouchingClimable) {
            if (playerInputs.shouldLimitKeyPresses) {
                playerInputs.IsPlayerStillClimbing(ref hasClimbedUp, ref hasClimbedDown);
            }
        }
        // Does the same check again but for the vines
        if (ropeControls.Attached && playerInputs.shouldLimitKeyPresses) {
            //playerInputs.IsPlayerStillClimbing(ref hasClimbedUp, ref hasClimbedDown);
            playerInputs.IsPlayerStillClimbingUp(ref hasClimbedUp);     // Player Cant Climb Down now
            //playerInputs.IsPlayerStillClimbingUp(ref hasJumped);     // Player Cant Climb Down now and player can only jump or

        }

        if (HaveAllKeysBeenUsed() && autoResetLevel && !isDead) {
            Invoke("ResetPlayer", resetTime);
            isDead = true;


        }

        // Switched to doing this in Update instead of fixed. Solves the sound not always playing for every jump when holding the jump button 
        // .. dont know if it maybe broke other things.
        isGrounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                if (!ropeControls.Attached) {
                    isGrounded = true;
                    anim.SetBool("IsJumping", false);

                }

            }
        }

    }


    //This is the version where the player can jump while the jump button is pressed down.
    //they can also jump and climb in the same level
    public void Move(float move, float climbUp, float climbDown, bool jump) {

        // Do Rope Stuff
        if (ropeControls.Attached) {
            //Changing Controls so that instead of climbing down the vine the player jumps off.
            // This means that the jump inpu no longer does anything
            // and the climb Down input jumps off input jumps off
            // Basically Climb down now mean drop/jump off vine
            jump = false;
            if (climbDown < 0 && !hasClimbedDown) {
                jump = true;        // jump is not what happens when the player hits the climb down button
            }
            climbDown = 0;      // the player cannot climb down

            if (hasClimbedUp && playerInputs.shouldLimitKeyPresses) {
                climbUp = 0;
            }

            SetClimingAnimations(climbUp, climbDown);

            ropeControls.PlayerRopeMovement(move, climbUp, climbDown, jump);
            if (jump && !isTouchingClimable && canJumpFromVine) {
                if (playerInputs.shouldLimitKeyPresses) {
                    hasClimbedDown = true;
                    UIManager.Instance.UsedClimbDown();
                }
                Jump(true, vineJumpMultiplier);
            }
            return;
        }
        else {
            isClimbing = false;
            anim.SetBool("IsClimbing", false);
        }

        TouchingClimable(climbUp, climbDown);
        HorizontalMovement(move);
        
        // If the player should jump...
        if (isGrounded && jump && !isTouchingClimable && !hasJumped) {

            Jump();
        }
    }

    private void HorizontalMovement(float move) {
        //only control the player if grounded or airControl is turned on
        if (isGrounded || airControl) {


            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rb2D.velocity.y);
            Vector3 velocity = Vector3.zero;
            // And then smoothing it out and applying it to the character

            rb2D.velocity = Vector3.SmoothDamp(rb2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            if (move > 0.1f || move < -0.1f) {
                anim.SetBool("IsRunning", true);
            }
            else {
                anim.SetBool("IsRunning", false);

            }
            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !isFacingRight) {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && isFacingRight) {
                // ... flip the player.
                Flip();
            }
        }
    }

    private void Jump(bool ignoreJumpLimit = false, float jumpForceMultiplier = 1) {      // jump force multiplier can be used if the if a different jump force is required when
        print("Jump");                                               // .. jumping from certain surfaces eg. ropes. Also ignore jumps from rope
        bool test = anim.GetBool("IsJumping");
        if (isGrounded) {
            //anim.Play("Idle");
            anim.StopPlayback();
        }
        // Add a vertical force to the player.
        isGrounded = false;
        rb2D.AddForce(new Vector2(0f, jumpForce * jumpForceMultiplier));
        anim.Play("Jump");
        anim.SetBool("IsJumping", true);
        if (playerInputs.shouldLimitKeyPresses && !ignoreJumpLimit) {
            hasJumped = true;
            UIManager.Instance.UsedJump();
        }
    }

    private void TouchingClimable(float climbUp, float climbDown) {
        if (isTouchingClimable) {

            // If the player touched the ladder while not on the ground. Immedielty switch to the climbing animation
            if (!isGrounded) {
                isClimbing = true;
                anim.Play("Climb");
                anim.SetBool("IsClimbing", true);
            }

            float inputVertical = 0;
            if (!hasClimbedUp) {             // Has the player already climbed up and stopped climbing up
                inputVertical += climbUp;
            }
            if (!hasClimbedDown) {          //Has the player already climbed down and stopped climbing down
                inputVertical += climbDown;
            }

            SetClimingAnimations(climbUp, climbDown);

            rb2D.velocity = new Vector2(rb2D.velocity.x, inputVertical * climbSpeed);
            rb2D.gravityScale = 0;
        }
        else {
            rb2D.gravityScale = 3;       // reset the gravity when the player loses contact with the ladder 
            isClimbing = false;
            anim.SetBool("IsClimbing", false);
            anim.speed = 1;
            // Stop Climbing Sound
            playerSounds.ClimbingSource.Stop();
        }
    }

    private void SetClimingAnimations(float climbUp, float climbDown) {
        if (climbUp != 0 || climbDown != 0) {
            anim.Play("Climb");
            anim.SetBool("IsClimbing", true);
        }
        else if (!isGrounded) {
            anim.Play("IdleClimb");
            anim.SetBool("IsClimbing", true);
        }
    }


    private void Flip() {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        // Flip the canvas Scale
        canvas.transform.localScale = theScale;
    }

    public void ResetPlayer() {
        // Reset booleans which track player inputs
        hasReleasedLeft = false;
        hasReleasedRight = false;
        hasClimbedUp = false;
        hasClimbedDown = false;
        hasJumped = false;

        ropeControls.Detach();
        transform.position = startPosition;             // Reset Player position
        rb2D.velocity = Vector3.zero;
        playerInputs.enabled = true;                    // Allow the player to make inputs
        isDead = false;
        anim.Play("Idle");
        UIManager.Instance.ResetUI();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable")) {
            isTouchingClimable = true;
        }
        if (collision.CompareTag("Water")) {
            OnPlayerDeath();
        }
        /*if (collision.CompareTag("SpinningPlatform")){                   // Parent player to spinning platform 
            this.transform.parent = collision.transform;
        }*/
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable")) {
            isTouchingClimable = false;
            isClimbing = false;
            if (Input.GetButton("VerticalUp") && playerInputs.shouldLimitKeyPresses) {
                hasClimbedUp = true;
                UIManager.Instance.UsedClimbUp();
            }
            /*if (collision.CompareTag("SpinningPlatform")){                   // Un-parent player to spinning platform 
                transform.parent = null;
            }*/
        }
    }


    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag("SpinningPlatform"))
            this.transform.parent = col.transform;
    }

    void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.CompareTag("SpinningPlatform"))
            this.transform.parent = null;
    }

    // Will Return True if all the players actions have been performed
    private bool HaveAllKeysBeenUsed() {
        if (hasReleasedLeft && hasReleasedRight
            && hasClimbedUp && hasClimbedDown
            && hasJumped
            ) {
            return true;
        }
        return false;
    }

    public void OnPlayerDeath() {
        if (isDead)
            return;
        isDead = true;
        anim.Play("Hurt");
        playerInputs.enabled = false;
        Invoke("ResetPlayer", resetTime);
    }

}
