using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkingPlatform : MonoBehaviour {
    
    [SerializeField] private bool startOn = true;
    [SerializeField] private bool autoSetColliderSize = true;
    [SerializeField] private float startDelayTime = 0;
    [SerializeField] private float onTime = 1f;     // the time the platform will be blinked on for
    [SerializeField] private float offTime = 1f;    // the time the blatform will be off for

    BoxCollider2D boxCollider;
    SpriteRenderer[] platformSprites;


    // Start is called before the first frame update
    void Start() {
        boxCollider = GetComponent<BoxCollider2D>();
        platformSprites = GetComponentsInChildren<SpriteRenderer>();
        SetBoxColliderOffsetAndSize();
        StartCoroutine(BlinkCycle());
    }

    private IEnumerator BlinkCycle() {
        yield return new WaitForSeconds(startDelayTime);

        if (startOn) {
            TurnOn();
            yield return new WaitForSeconds(onTime);
        }

        while (true) {
            TurnOff();
            yield return new WaitForSeconds(offTime);
            TurnOn();
            yield return new WaitForSeconds(onTime);
        }
    }

    private void TurnOn() {
        boxCollider.enabled = true;
        foreach (var sprite in platformSprites) {
            sprite.enabled = true;
        }
    }

    private void TurnOff() {
        boxCollider.enabled = false;
        foreach (var sprite in platformSprites) {
            sprite.enabled = false;
        }
    }

    // This method automatically sets the offset and size of the box collider depending on how many sprites make up the platform.
    // it assumes the sprites are 1 grid unit wide
    private void SetBoxColliderOffsetAndSize() {
        if (!autoSetColliderSize) { return; }
        float offsetX = (float)(platformSprites.Length) / 2 - 0.5f;
        Vector2 offset = new Vector2(offsetX, boxCollider.offset.y);
        boxCollider.offset = offset;

        Vector2 size = new Vector2(platformSprites.Length, boxCollider.size.y);
        boxCollider.size = size;
    }
}
