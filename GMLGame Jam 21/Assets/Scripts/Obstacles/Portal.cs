using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] private Transform targetPortal;
    [SerializeField] private AudioSource portalSource;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.transform.position = targetPortal.position;
            portalSource.pitch = Random.Range(0.85f, 1.1f);
            portalSource.Play();
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, targetPortal.position);
    }
}
