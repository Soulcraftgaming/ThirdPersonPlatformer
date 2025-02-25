using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Rigidbody capsuleRigidbody;
    public float moveSpeed = 5f;  // Increase speed slightly for better control
    public float jumpPower = 5f; 
    public bool isGrounded = false;

    void Start()
    {
        // Ensuring Rigidbody constraints to prevent tipping
        capsuleRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    void Update()
    {
        Vector3 moveDirection = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) {
            moveDirection += Vector3.forward;
        }
        
        if (Input.GetKey(KeyCode.A)){
            moveDirection += Vector3.left;
        }
        
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back;
        }
        
        
        if (Input.GetKey(KeyCode.D)) 
        {
            moveDirection += Vector3.right;
        } 

        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            capsuleRigidbody.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }

        // Moving the capsule
        capsuleRigidbody.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Acceleration);
    }
}
