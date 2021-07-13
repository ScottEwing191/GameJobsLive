using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CharacterController2D : MonoBehaviour {
    [SerializeField] private float jumpForce = 400f;                            // Amount of force added when the player jumps.
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;    // How much to smooth out the movement
    [SerializeField] private float climbSpeed = 100f;                               // How fast can the player climb
    [SerializeField] private bool airControl = false;                           // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask whatIsGround;                          // A mask determining what is ground to the character

    [SerializeField] private Transform groundCheck;                         // A position marking where to check if the player is grounded.
    [SerializeField] private Transform ceilingCheck;                            // A position marking where to check for ceilings
    [SerializeField] private bool autoResetLevel = false;                   // should the player automatically reset once the player has used all their actions

    private float resetTime = 2;                // The time after the player pressed the last key before the game resets
    const float groundedRadius = .2f;           // Radius of the overlap circle to determine if grounded

    private bool isGrounded;                 // Whether or not the player is grounded.
    private bool isFacingRight = true;          // For determining which way the player is currently facing.  
    private bool isTouchingClimable = false;    // is the player touching a surface they can climb up
    private bool isClimbing = false;            // has the player actually started climbing

    //Movement Tracking Variables
    private bool hasReleasedRight = false;
    private bool hasReleasedLeft = false;
    private bool hasClimbedUp = false;           // Has the player already climbed and stopped climbing i.e. pressed and released the climb buttons (while able to climb)
    private bool hasClimbedDown = false;
    private bool hasJumped;

    private Vector3 startPosition;              // The position that the player will return to when the player Resets
    private Rigidbody2D rigidbody2D;
    private PlayerInputs playerInputs;
    private Animator anim;



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
    // === PROPERTIES END

    private void Awake() {
        rigidbody2D = GetComponent<Rigidbody2D>();
        playerInputs = GetComponent<PlayerInputs>();
        anim = GetComponent<Animator>();
        startPosition = transform.position;

    }

    private void Update() {
        // If the player is touching the ladder check every frame if the player has released the climb up or down buttons. 
        if (isTouchingClimable) {
            if (playerInputs.shouldLimitKeyPresses) {
                playerInputs.IsPlayerStillClimbing(ref hasClimbedUp, ref hasClimbedDown);
            }
        }

        if (HaveIfAllKeysUsed() && autoResetLevel) {
            Invoke("ResetPlayer", 3f);


        }
    }


    private void FixedUpdate() {
        //bool wasGrounded = m_Grounded;
        isGrounded = false;
        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i].gameObject != gameObject) {
                isGrounded = true;
                anim.SetBool("IsJumping", false);

            }
        }


    }


    public void Move(float move, float climbUp, float climbDown, bool jump) {


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

            if (inputVertical != 0) {
                anim.speed = 1;
                anim.SetBool("IsClimbing", true);
                isClimbing = true;
            }
            else if (isClimbing) {
                anim.speed = 0;
            }
            rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, inputVertical * climbSpeed);
            rigidbody2D.gravityScale = 0;
        }
        else {
            rigidbody2D.gravityScale = 3;       // reset the gravity when the player loses contact with the ladder 
            isClimbing = false;
            anim.SetBool("IsClimbing", false);
            anim.speed = 1;


        }

        //only control the player if grounded or airControl is turned on
        if (isGrounded || airControl) {


            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(move * 10f, rigidbody2D.velocity.y);
            Vector3 velocity = Vector3.zero;
            // And then smoothing it out and applying it to the character

            rigidbody2D.velocity = Vector3.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, movementSmoothing);

            /*if (move > 0.1f || move < -0.1f) {
                anim.SetBool("IsRunning", true);
            }
            else {
                anim.SetBool("IsRunning", false);

            }*/

            if (velocity.x > 0.1f || velocity.x < -0.1f) {
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
        // If the player should jump...
        if (isGrounded && jump && !isTouchingClimable && !hasJumped) {
            // Add a vertical force to the player.
            isGrounded = false;
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            anim.SetBool("IsJumping", true);
            if (playerInputs.shouldLimitKeyPresses) {
                hasJumped = true;
            }
        }
    }


    private void Flip() {
        // Switch the way the player is labelled as facing.
        isFacingRight = !isFacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void ResetPlayer() {
        hasReleasedLeft = false;
        hasReleasedRight = false;
        hasClimbedUp = false;
        hasClimbedDown = false;
        hasJumped = false;
        transform.position = startPosition;
        rigidbody2D.velocity = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable")) {
            isTouchingClimable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Climbable")) {
            isTouchingClimable = false;
            isClimbing = false;
            if (Input.GetButton("VerticalUp") && playerInputs.shouldLimitKeyPresses) {
                hasClimbedUp = true;
            }
        }
    }

    // Will Return True if all the players actions have been performed
    private bool HaveIfAllKeysUsed() {
        if (hasReleasedLeft && hasReleasedRight
            && hasClimbedUp && hasClimbedDown
            && hasJumped
            ) {
            return true;
        }
        return false;
    }

}
