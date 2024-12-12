using UnityEngine;
using UnityEngine.InputSystem;

public class BracedHang : MonoBehaviour
{
    [SerializeField] private LayerMask edgeLayer; // Layer to detect edges
    [SerializeField] private float detectionDistance = 1.5f; // Distance to detect edges
    [SerializeField] private InputActionReference hangAction; // Input action for braced hang

    private Animator anim;
    private bool isNearEdge = false; // Track if near edge
    [SerializeField] private Transform detectedEdge; // Reference to the detected edge
    [SerializeField] Transform pos;
    bool hasTriggeredHang = false; // Prevent re-triggering

    Jump jump;
    PlayerController controller;
    private void Start()
    {
        anim = GetComponent<Animator>();
        jump = GetComponent<Jump>();
        controller = GetComponent<PlayerController>();

        // Enable the input action
        hangAction.action.Enable();
    }

    private void Update()
    {
        DetectEdge();

        //if(isNearEdge && jump.isJumping)
        //{
        //    PerformBracedHang();
        //}

        if (isNearEdge)
        {
            PerformBracedHang();
        }

        if (IsAnimationStateStarted("Crouch To Stand"))
        {
            transform.position = pos.position;
            hasTriggeredHang = false;
            controller.isMove = true;

            Debug.Log("Set New Top Position!");
        }
    }

    private void PerformBracedHang()
    {
        if (detectedEdge != null && !hasTriggeredHang)
        {
            anim.SetTrigger("Hang"); // Trigger animation
            hasTriggeredHang = true;
            // Calculate the hang position based on the edge's position

            controller.isMove = false;
            
        }
    }
    private void DetectEdge()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, detectionDistance, edgeLayer))
        {
            isNearEdge = true;

            detectedEdge = hit.transform;
        }
        else
        {
            isNearEdge = false;
            detectedEdge = null;
        }

        Debug.DrawRay(transform.position, transform.forward * detectionDistance, isNearEdge ? Color.green : Color.red);
    }

    private void OnDisable()
    {
        // Disable the input action when the object is disabled
        hangAction.action.Disable();
    }
    private bool IsAnimationStateCompleted(string stateName)
    {
        // Get the Animator's current state info
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0); // 0 is the base layer

        // Check if the current state matches the desired state and has completed
        return currentState.IsName(stateName) && currentState.normalizedTime >= 1.0f;
    }
    private bool IsAnimationStateStarted(string stateName)
    {
        // Get the Animator's current state info
        AnimatorStateInfo currentState = anim.GetCurrentAnimatorStateInfo(0); // 0 is the base layer

        // Check if the current state matches the desired state and has completed
        return currentState.IsName(stateName) && currentState.normalizedTime < 1f;
    }
}
