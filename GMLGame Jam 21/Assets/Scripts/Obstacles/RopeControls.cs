using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeControls : MonoBehaviour {
    private Rigidbody2D playerRb;
    private HingeJoint2D playerHj;

    [SerializeField] float pushForce = 10f;
    [SerializeField] float climbSpeed = 5;
    [Range(0, 600)] [SerializeField] private float ropeGrabForce = 250;    // When the player grabs the rope. the player's velocity is multiplied by..
                                                                           // .. by this value and added to the velocity of the rope secment the player
                                                                           // .. grabbed


    private bool attached = false;
    private Transform attachedTo;
    private GameObject disregard;
    private bool canClimb = true;
    private float climbWaitTime = 0.25f;
    private float disregardTime = 2;

    // Properties
    public bool Attached {
        get { return attached; }
        set { attached = value; }
    }


    private void Awake() {
        playerRb = GetComponentInParent<Rigidbody2D>();
        playerHj = GetComponentInParent<HingeJoint2D>();
    }

    public void PlayerRopeMovement(float swing, float climbUp, float climbDown, bool detach) {
        if (swing < 0 && attached) {
            playerRb.AddRelativeForce(Vector2.left * pushForce);
        }
        else if (swing > 0 && attached) {
            playerRb.AddRelativeForce(Vector2.right * pushForce);
        }
        if (climbUp > 0 && attached && canClimb) {
            // Incres the connected anchor Y by an ammount
            float newConnectedAnchorY = playerHj.connectedAnchor.y + (climbSpeed * Time.fixedDeltaTime);
            playerHj.connectedAnchor = new Vector2(0, newConnectedAnchorY);
            // Once the connected anchor Y value isalmost at the length of the rope segment sprite
            RopeSegment myConnection = playerHj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
            
            float spriteBottom = -myConnection.GetComponent<BoxCollider2D>().bounds.size.y;

            if (playerHj.connectedAnchor.y >= 0) {
                playerHj.connectedAnchor = new Vector2(0, spriteBottom);
                Slide(1);
            }
        }
        if (climbDown < 0 && attached && canClimb) {
            // Decrease the connected anchor Y by an ammount
            float newConnectedAnchorY = playerHj.connectedAnchor.y - (climbSpeed * Time.fixedDeltaTime);
            playerHj.connectedAnchor = new Vector2(0, newConnectedAnchorY);
            // Once the connected anchor Y value isalmost at the length of the rope segment sprite
            RopeSegment myConnection = playerHj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
            
            float spriteBottom = -myConnection.GetComponent<BoxCollider2D>().bounds.size.y;

            if (playerHj.connectedAnchor.y <= spriteBottom) {
                playerHj.connectedAnchor = Vector2.zero;
                Slide(-1);
            }
        }
        if (detach) {         // doesnt work dint up key is jump/detatch and climb up
            Detach();
        }
    }

    public void Attach(Rigidbody2D ropeBone) {
        Vector2 playerVelocity = playerRb.velocity;
        ropeBone.gameObject.GetComponent<RopeSegment>().IsPlayerAttached = true;
        playerHj.connectedBody = ropeBone;
        playerHj.enabled = true;
        ropeBone.AddRelativeForce(playerVelocity * 250);          // add the player velocity to the rope when the player jumps on
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
    }


    public void Detach() {
        if (playerHj.connectedBody == null) {
            return;
        }
        playerHj.connectedBody.gameObject.GetComponent<RopeSegment>().IsPlayerAttached = false;
        attached = false;
        playerHj.enabled = false;
        playerHj.connectedBody = null;
        disregard = attachedTo.gameObject;
        attachedTo = null;

        StartCoroutine(ClearDisregardTimer());          // wait a second after detaching from the Rope before allowing the player to re-attach
                                                        // .. if the the player detaches at the top they have time to fall without re attaching
    }

    private void Slide(int direction) {
        RopeSegment myConnection = playerHj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
        GameObject newSeg = null;
        if (direction > 0) {    // Slide up
            if (myConnection.connectedAbove != null) {      // Not on top segment of Rope
                if (myConnection.connectedAbove.gameObject.GetComponent<RopeSegment>() != null) {
                    newSeg = myConnection.connectedAbove;
                }
            }
        }
        else if (myConnection.connectedBelow !=null ){
            newSeg = myConnection.connectedBelow;
        }
        else {
            Detach();       // Fall of the bottom of the rope
        }
        if (newSeg != null) {
            transform.parent.transform.position = newSeg.transform.position;
            myConnection.IsPlayerAttached = false;
            newSeg.GetComponent<RopeSegment>().IsPlayerAttached = true;
            playerHj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
       
    }

    private IEnumerator ClimbWaitTimer() {
        canClimb = false;
        yield return new WaitForSeconds(climbWaitTime);
        canClimb = true;
    }

    private IEnumerator ClearDisregardTimer() {
        yield return new WaitForSeconds(disregardTime);
        disregard = null;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!attached && collision.CompareTag("Rope")) {
            if (attachedTo != collision.gameObject.transform.parent) {      // if the previous attached rope is not the same as the current rope
                if (disregard == null || collision.gameObject.transform.parent.gameObject != disregard) {  // if rope to disgard is not the same as.. 
                                                                                                           // colliders parent gameobject
                    Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                }
            }
        }

        // If the player is attached to a rope and they collide with another rope they should detach from the current rope and attach onto the next one
        if (attached && collision.gameObject.CompareTag("Rope")) {
            if (attachedTo != collision.gameObject.transform.parent) {  // if not colliding to the currently attached rope
                if (disregard == null || collision.gameObject.transform.parent.gameObject != disregard) {   // if not colliding with rope just detached from
                    Detach();
                    Attach(collision.gameObject.GetComponent<Rigidbody2D>());
                }
            }
        }
    }
}
