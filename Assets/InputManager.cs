using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();
    public UnityEvent OnJumpPressed = new UnityEvent();
    public UnityEvent OnDash = new UnityEvent();
    public Transform cameraTransform;

    private int jumpCount = 0;
    private const int maxJumps = 2;
    public UnityEvent OnLand = new UnityEvent(); // Event triggered when landing

    void Start()
    {
        OnLand.AddListener(ResetJumpCount);
    }

    void Update()
    {
        // Handle movement input
        Vector2 input = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) input += Vector2.up;
        if (Input.GetKey(KeyCode.A)) input += Vector2.left;
        if (Input.GetKey(KeyCode.S)) input += Vector2.down;
        if (Input.GetKey(KeyCode.D)) input += Vector2.right;

        // Normalize input to prevent faster diagonal movement
        if (input.magnitude > 1f) input.Normalize();

        // Convert input to world space based on camera direction
        Vector3 moveDirection = Vector3.zero;
        if (cameraTransform != null)
        {
            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;
            forward.y = 0; // Ignore vertical tilt
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            moveDirection = (forward * input.y + right * input.x).normalized;
        }
        else
        {
            moveDirection = new Vector3(input.x, 0, input.y);
        }

        // Convert to Vector2 before invoking
        OnMove?.Invoke(new Vector2(moveDirection.x, moveDirection.z));

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space) && jumpCount < maxJumps)
        {
            jumpCount++;
            OnJumpPressed?.Invoke();
        }

        // Handle dash input
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnDash?.Invoke();
        }
    }

    private void ResetJumpCount()
    {
        jumpCount = 0;
    }

    public void NotifyLanding()
    {
        ResetJumpCount();
        OnLand?.Invoke();
    }
}
