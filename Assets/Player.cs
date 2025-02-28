using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 3f;

    private Rigidbody rb;
    private bool isGrounded = true;
    private bool canDash = true;
    private bool isDashing = false;
    private Transform cameraTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Camera cam = Camera.main ?? FindFirstObjectByType<Camera>();
        if (cam != null)
        {
            cameraTransform = cam.transform;
        }
        else
        {
            Debug.LogError("No Camera found in the scene! Make sure there is a camera tagged as 'MainCamera'.");
        }

        if (inputManager == null)
        {
            inputManager = FindFirstObjectByType<InputManager>();
            if (inputManager == null)
            {
                Debug.LogError("No InputManager found in the scene!");
                return;
            }
        }

        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnJumpPressed.AddListener(Jump);
        inputManager.OnDash.AddListener(Dash);
    }

    private void MovePlayer(Vector2 direction)
    {
        if (isDashing) return; // Prevent movement while dashing
        if (direction.magnitude == 0) return; 

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = (cameraForward * direction.y + cameraRight * direction.x).normalized;
        Vector3 velocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, moveDirection.z * speed);
        rb.linearVelocity = velocity;
    }

    private void Jump()
    {
        if (isGrounded || inputManager.OnLand.GetPersistentEventCount() < 2) // Allow double jump
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
            isGrounded = false;
        }
    }

    private void Dash()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        Vector3 dashDirection = cameraTransform.forward;
        dashDirection.y = 0;
        dashDirection.Normalize();

        rb.linearVelocity = dashDirection * dashForce;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Player Ground Collision
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            inputManager.NotifyLanding();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.AddScore(1);
            Destroy(other.gameObject);
        }
    }
}
