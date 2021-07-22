using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitchedRopeSegment : RopeSegment
{
    [SerializeField] private Rigidbody2D hitch;
    HingeJoint2D hitchHinge;
    HitchedRope rope;
    
    public HingeJoint2D HitchHinge {
        get { return hitchHinge; }
        set { hitchHinge = value; }
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        CreateHitch();
    }

    public void CreateHitch() {
        rope = transform.parent.GetComponentInParent<HitchedRope>();
        if (hitch != null) {
            hitchHinge = gameObject.AddComponent(typeof(HingeJoint2D)) as HingeJoint2D;
            hitchHinge.autoConfigureConnectedAnchor = false;
            hitchHinge.connectedBody = hitch;
            //hitchHinge.connectedAnchor = new Vector2(0, -0.5f);
            hitchHinge.connectedAnchor = new Vector2(0, 0);
            hitchHinge.useLimits = true;
            JointAngleLimits2D newLimits = hitchHinge.limits;
            newLimits.min = 0;
            newLimits.max = 0;
            hitchHinge.limits = newLimits;

            rope.HitchedSegment = this;
            rope.HitchedSegmentHinge = hitchHinge;
        }
        //Invoke("StopVelocity", 3);
    }

    public void SetHitch() {
        hitchHinge.enabled = true;
    }

    private void StopVelocity() {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("RopeCollider")) {
            rope.ReleaseHitch();
        }
    }


}
