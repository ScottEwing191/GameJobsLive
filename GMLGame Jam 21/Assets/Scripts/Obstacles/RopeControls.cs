using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeControls : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    private HingeJoint2D hj;

    [SerializeField] float pushForce = 10f;
    [SerializeField] float climbSpeed = 5;
    [Range(0, 600)] [SerializeField] private float ropeGrabForce = 250;    // When the player grabs the rope. the player's velocity is multiplied by..
                                                                           // .. by this value and added to the velocity of the rope secment the player
                                                                           // .. grabbed


    [SerializeField] private bool attached = false;
    [SerializeField] private Transform attachedTo;
    private GameObject disregard;
    private bool canClimb = true;
    private float climbWaitTime = 0.25f;

    // Properties
    public bool Attached {
        get { return attached; }
        set { attached = value; }
    }


    private void Awake() {
        rb = GetComponentInParent<Rigidbody2D>();
        hj = GetComponentInParent<HingeJoint2D>();
    }

    public void PlayerRopeMovement(float swing, float climbUp, float climbDown, bool detach) {
        if (swing < 0 && attached) {
            rb.AddRelativeForce(Vector2.left * pushForce);
        }
        else if (swing > 0 && attached) {
            rb.AddRelativeForce(Vector2.right * pushForce);
        }
        if (climbUp > 0 && attached && canClimb) {
            // Incres the connected anchor Y by an ammount
            float newConnectedAnchorY = hj.connectedAnchor.y + (climbSpeed * Time.fixedDeltaTime);
            hj.connectedAnchor = new Vector2(0, newConnectedAnchorY);
            // Once the connected anchor Y value isalmost at the length of the rope segment sprite
            RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
            
            float spriteBottom = -myConnection.GetComponent<BoxCollider2D>().bounds.size.y;

            if (hj.connectedAnchor.y >= 0) {
                hj.connectedAnchor = new Vector2(0, spriteBottom);
                Slide(1);
            }
        }
        if (climbDown < 0 && attached && canClimb) {
            // Decrease the connected anchor Y by an ammount
            float newConnectedAnchorY = hj.connectedAnchor.y - (climbSpeed * Time.fixedDeltaTime);
            hj.connectedAnchor = new Vector2(0, newConnectedAnchorY);
            // Once the connected anchor Y value isalmost at the length of the rope segment sprite
            RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
            
            float spriteBottom = -myConnection.GetComponent<BoxCollider2D>().bounds.size.y;

            if (hj.connectedAnchor.y <= spriteBottom) {
                hj.connectedAnchor = Vector2.zero;
                Slide(-1);
            }
        }
        if (detach) {         // doesnt work dint up key is jump/detatch and climb up
            Detach();
        }
    }

    public void Attach(Rigidbody2D ropeBone) {
        Vector2 playerVelocity = rb.velocity;
        ropeBone.gameObject.GetComponent<RopeSegment>().IsPlayerAttached = true;
        hj.connectedBody = ropeBone;
        hj.enabled = true;
        ropeBone.AddRelativeForce(playerVelocity * 250);          // add the player velocity to the rope when the player jumps on
        attached = true;
        attachedTo = ropeBone.gameObject.transform.parent;
    }


    private void Detach() {
        //Vector2 playerVelocity = rb.velocity;
        hj.connectedBody.gameObject.GetComponent<RopeSegment>().IsPlayerAttached = false;
        attached = false;
        hj.enabled = false;
        hj.connectedBody = null;
        disregard = attachedTo.gameObject;
        attachedTo = null;
        //rb.velocity = playerVelocity;
        StartCoroutine(ClearDisregardTimer());          // wait a second after detaching from the Rope before allowing the player to re-attach
                                                        // .. if the the player detaches at the top they have time to fall without re attaching
    }

    private void Slide(int direction) {
        RopeSegment myConnection = hj.connectedBody.gameObject.GetComponent<RopeSegment>();     // Rope Segment currently Connected to
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
            hj.connectedBody = newSeg.GetComponent<Rigidbody2D>();
        }
       
    }

    private IEnumerator ClimbWaitTimer() {
        canClimb = false;
        yield return new WaitForSeconds(climbWaitTime);
        canClimb = true;
    }

    private IEnumerator ClearDisregardTimer() {
        yield return new WaitForSeconds(1);
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
    }
}
