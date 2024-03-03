using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class shootBasketball : MonoBehaviour
{
    public float pushForce = 10f; // The force with which the ball will be pushed.
    private XRGrabInteractable _grabInteractable; // Reference to the XRGrabInteractable component.


    [SerializeField]
    private LineRenderer LineRenderer;



    [Header("Display Controls")]
    [SerializeField]
    [Range(10,100)]
    private int LinePoints = 25;

    [SerializeField]
    [Range(0.01f,0.25f)]

    private Rigidbody rb;
    private float TimeBetweenPoints = 0.1f;
    private void Awake()
    {
        // Get the XRGrabInteractable component from the GameObject.
        _grabInteractable = GetComponent<XRGrabInteractable>();
        // get rigidbody mass
        rb = GetComponent<Rigidbody>();

    }

    private void Update()
    {
        if (_grabInteractable.isSelected)
        {
            DrawProjection();
        }
        else
        {
            LineRenderer.enabled = false;
        }
    }

    private void DrawProjection()
    {
        LineRenderer.enabled = true;
        LineRenderer.positionCount = Mathf.CeilToInt(LinePoints/TimeBetweenPoints) + 1;
        Vector3 startPosition = transform.position;
        Vector3 startVelocity = pushForce * transform.forward/rb.mass ; // mudei aqui CHECAR AQUI SE DER PROBLEMA
        int i = 0;
        LineRenderer.SetPosition(i,startPosition);
        for (float time = 0; time< LinePoints; time+=TimeBetweenPoints)
        {
            i++;
            Vector3 point = startPosition + time*startVelocity;
            point.y = startPosition.y + startVelocity.y*time + (0.5f*Physics.gravity.y*time*time);
            LineRenderer.SetPosition(i,point);
        }
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
