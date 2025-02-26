using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();
    public UnityEvent OnJumpPressed = new UnityEvent();

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

        // Invoke movement event
        OnMove?.Invoke(input);

        // Handle jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpPressed?.Invoke();
        }
    }
}
