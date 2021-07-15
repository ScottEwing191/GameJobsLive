using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEndPoint : MonoBehaviour
{
    private bool levelAlreadyEnding = false;        // stops the LevelComplete Method being called twice. One for each of the colliders on the player
    private void OnTriggerEnter2D(Collider2D collision) {  
        if (collision.CompareTag("Player") && !levelAlreadyEnding) {
            levelAlreadyEnding = true;  
            GameManager.Instance.LevelComplete();

        }
    }
}
