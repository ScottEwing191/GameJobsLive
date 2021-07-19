using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Rigidbody2D hook;
    public GameObject[] prefabRopeSegs;
    public int numLinks = 6;
    private int[] segmentOrder = { 0, 1, 2, 2, 3, 2, 2, 3, 2, 4, 4, 5 }; 
    // Start is called before the first frame update
    void Start()
    {
        GenerateRope();
    }

    private void GenerateRope() {
        Rigidbody2D prevBody = hook;
        int index = 0;
        for (int i = 0; i < numLinks; i++) {
            //int index = UnityEngine.Random.Range(0, prefabRopeSegs.Length);

            if (i == 0) {                       // Set the top segment
                index = 0;
            }else if(i == 1) {                  // The second secoment only fits after the first
                index = 1;
            }
            else if (i == numLinks - 2) {
                index = 4;
            }
            else if(i == numLinks - 1) {       // Set the last Segment
                index = 5;
            }
            else {
                do {
                    index = UnityEngine.Random.Range(0, prefabRopeSegs.Length);

                } while (index == 0 || index == 1 || index == 5);           // Dont Select any of the segments from the if statement

               
            }

            

            GameObject newSeg = Instantiate(prefabRopeSegs[index]);
            newSeg.transform.parent = transform;
            newSeg.transform.position = transform.position;
            HingeJoint2D hj = newSeg.GetComponent<HingeJoint2D>();
            hj.connectedBody = prevBody;

            prevBody = newSeg.GetComponent<Rigidbody2D>();
        }

    }
}
