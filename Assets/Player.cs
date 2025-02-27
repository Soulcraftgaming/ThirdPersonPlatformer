using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Get the main camera transform safely
        Camera cam = Camera.main ?? FindFirstObjectByType<Camera>();
        if (cam != null)
        {
            cameraTransform = cam.transform;
        }
        else
        {
            Debug.LogError("No Camera found in the scene! Make sure there is a camera tagged as 'MainCamera'.");
        }

        // Ensure InputManager is assigned
        if (inputManager == null)
        {
            inputManager = FindFirstObjectByType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("No InputManager found in the scene!");
                return;
            }
        }

        // Subscribe to input events
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJumpPressed.AddListener(Jump);
    }

    private void MovePlayer(Vector2 direction)
    {
        if (direction.magnitude == 0) return; // No movement if no input

        // Get the camera's forward and right vectors, but ignore the Y-axis
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Convert input direction to world space relative to the camera
        Vector3 moveDirection = (cameraForward * direction.y + cameraRight * direction.x).normalized;

        // Apply force for movement while keeping Y-axis velocity unchanged
        Vector3 velocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        if (inputManager != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            inputManager.NotifyLanding(); // Reset jump count when touching the ground
        }
    }
}
