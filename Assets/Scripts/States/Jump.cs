using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer; // Layer to detect edges
    [SerializeField] private float jumpForce = 5f; // Distance to detect edges
    [SerializeField] private InputActionReference jumpAction; // Input action for braced hang

    private Animator anim;
    Rigidbody rb;
    private bool isGrounded = false; // Track if near edge
    public bool isJumping = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        // Enable the input action
        jumpAction.action.Enable();
    }

    private void Update()
    {
        // Check for input and trigger the hang
        if (isGrounded && jumpAction.action.WasPerformedThisFrame())
        {
            PerformJump();
        }
    }

    private void PerformJump()
    {
        Debug.Log("Performing Braced Hang!");
        anim.SetTrigger("Jump"); // Trigger animation
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
        }
    }

    private void OnDisable()
    {
        // Disable the input action when the object is disabled
        jumpAction.action.Disable();
    }
}
