using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitchedRope : MonoBehaviour {
    public Rigidbody2D hook;
    private HitchedRopeSegment hitchedSegment;
    private HingeJoint2D hitchedSegmentHinge;

    public HitchedRopeSegment HitchedSegment {
        get { return hitchedSegment; }
        set { hitchedSegment = value; }
    }
    public HingeJoint2D HitchedSegmentHinge {
        get { return hitchedSegmentHinge; }
        set { hitchedSegmentHinge = value; }
    }

    public void ReleaseHitch() {
        if (hitchedSegmentHinge != null) {
            hitchedSegmentHinge.enabled = false;
        }
    }

    public void ResetHitch() {
        hitchedSegment.SetHitch();
    }
}
