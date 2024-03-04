using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopLogic : MonoBehaviour
{
    public Collider targetCollider; // Assign this via the Unity Editor to your target child collider

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider that triggered the event is interacting with the targetCollider
        if (targetCollider != null && other == targetCollider)
        {
            Debug.Log("The target collider has been hit by another object.");
            // Here, you can implement the logic that should occur when the target collider is hit
        }
    }
}
