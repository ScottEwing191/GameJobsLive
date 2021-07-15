using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpriteOnStart : MonoBehaviour
{
    [SerializeField] bool hideSprite = true;
    private void Awake() {
        if (hideSprite) {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }   
}
