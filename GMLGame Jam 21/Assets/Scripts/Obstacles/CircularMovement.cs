using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour {

	
	[SerializeField]
	Transform rotationCenter;
	[SerializeField]
	float rotationRadius = 2f, angularSpeed = 2f;
	[SerializeField] bool rotateAntiClockwise = false;
	float posX, posY, angle = 0f;
	float directionModifier = 1;

    private void Start() {
        if (rotateAntiClockwise) {
			directionModifier = -1;
        }
    }

    // Update is called once per frame
    void Update () {
		

		posX = rotationCenter.position.x + Mathf.Cos(-angle) * rotationRadius;
		posY = rotationCenter.position.y + Mathf.Sin(-angle) * rotationRadius;



		transform.position = new Vector2 (posX, posY);
		angle = angle + (directionModifier * Time.deltaTime * angularSpeed);

		if (angle >= 360f)
			angle = 0f;
	}
}
