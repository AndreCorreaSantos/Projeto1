using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class shootBasketball : MonoBehaviour
{
    public float pushForce = 10f; // The force with which the ball will be pushed.
    private XRGrabInteractable _grabInteractable; // Reference to the XRGrabInteractable component.

    private void Awake()
    {
        // Get the XRGrabInteractable component from the GameObject.
        _grabInteractable = GetComponent<XRGrabInteractable>();
    }

    // This function will push the basketball in the direction of its forward vector.
    public void PushForward()
    {
        // Ensure the ball is not being grabbed.
        if (_grabInteractable.isSelected)
        {
            // Force end the interaction with the interactor.
            _grabInteractable.interactionManager.SelectExit(_grabInteractable.selectingInteractor, _grabInteractable);
        }
        
        // Add the push force to the Rigidbody component.
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Apply a force in the forward direction of the basketball.
            rb.AddForce(transform.forward * pushForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("No Rigidbody attached to the basketball.");
        }
    }
}
