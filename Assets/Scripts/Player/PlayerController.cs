using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of movement
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private InputActionReference moveAction; // Input Action for movement
    [SerializeField] private Animator anim; // Animator reference
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float time;

    private Vector2 movementInput;
    private bool facingRight = true; // Track if the character is facing right
    public bool isMove = true;

    private float t = 0f; // Progress along the path
    PathSystem pathSystem;
    private void OnEnable()
    {
        // Enable the input action
        moveAction.action.Enable();
    }
    private void OnDisable()
    {
        // Disable the input action
        moveAction.action.Disable();
    }
    private void Start()
    {
        pathSystem = FindFirstObjectByType<PathSystem>();

        anim = GetComponent<Animator>();
        Debug.Log("Player has : " + pathSystem.GetPathManager().GetPaths().Count);

        isMove = true;
        pathSystem.GetPathManager().SetPlayerDefaultPosition(transform);
    }
    private void Update()
    {
        if (!isMove) return;

        // Get horizontal movement input
        movementInput = moveAction.action.ReadValue<Vector2>();

        // Move the player on the X-axis
        MovePlayerAlongPath();

        // Check and adjust character rotation
        HandleRotation();

        // Update animation parameters
        UpdateAnimation();
    }
    private void MovePlayer()
    {
        // Move the player left or right based on input
        Vector3 move = new Vector3(movementInput.x, 0, 0) * speed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
    private void MovePlayerAlongPath()
    {
        List<Vector3> paths = pathSystem.GetPathManager().GetPaths();

        if (paths.Count > 1)
        {
            // Update progress based on horizontal movement
            if (movementInput.x > 0 && t < 1f) // Moving forward (right)
            {
                t += speed * Time.deltaTime / paths.Count;
            }
            else if (movementInput.x < 0 && t > 0f) // Moving backward (left)
            {
                t -= speed * Time.deltaTime / paths.Count;
            }

            // Clamp t to ensure it stays within bounds (0 to 1)
            t = Mathf.Clamp01(t);

            // Calculate the player's position along the path
            Vector3 targetPosition = pathSystem.GetPathManager().GetPlayerPositionOnPath(t);

            // Move the player smoothly towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Not moving!");
        }
    }
    private void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();

        if(Physics.Raycast(ray, out info, whatIsGround))
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal),animCurve.Evaluate(time));
        }
    }
    private void HandleRotation()
    {
        // Get the current and next positions based on path progress
        var (previousPosition, nextPosition) = pathSystem.GetPathManager().GetAdjacentPoints(transform.position);

        // Check if positions are valid before rotating
        if (previousPosition.HasValue && nextPosition.HasValue)
        {
            // Determine if the player is moving forward or backward on the path
            if (movementInput.x > 0) // Moving forward (right)
            {
                RotateTowards(nextPosition.Value); // Rotate to face the previous position
            }
            else if (movementInput.x < 0) // Moving backward (left)
            {
                RotateTowards(previousPosition.Value); // Rotate to face the next position
            }
        }
    }
    private void RotateTowards(Vector3 targetPosition)
    {
        facingRight = !facingRight; // Toggle direction
        Vector3 direction = targetPosition - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }
    private void UpdateAnimation()
    {
        // Set Speed parameter based on horizontal input magnitude
        float speedParameter = Mathf.Abs(movementInput.x);
        anim.SetFloat("Speed", speedParameter);
    }
}
