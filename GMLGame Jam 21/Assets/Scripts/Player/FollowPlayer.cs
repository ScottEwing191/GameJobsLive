using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour 
{
    [SerializeField] private bool shouldFollowPlayer = true;
    [SerializeField] private Transform player;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;	// How much to smooth out the movement
    

    private Vector3 offset;
    // Start is called before the first frame update
    private void Awake() {
        if (player!= null) {
            offset = transform.position - player.position;
        }
    }

    void FixedUpdate()
    {
        if (shouldFollowPlayer) {
            
            Vector3 currentVelocity = Vector3.zero;
            transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref currentVelocity, movementSmoothing);
        }
    }


}
