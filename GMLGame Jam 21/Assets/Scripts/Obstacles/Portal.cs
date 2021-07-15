using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{

    [SerializeField] private Transform targetPortal;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            collision.transform.position = targetPortal.position;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, targetPortal.position);
    }
}
