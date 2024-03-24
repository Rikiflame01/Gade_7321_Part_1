using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float turnSpeed = 200f; // Speed of turning, can adjust as needed
    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        // Initialize the controls
        controls = new PlayerControls();

        // Bind the movement action
        controls.Player.WASD.performed += ctx =>
        {
            moveInput = ctx.ReadValue<Vector2>();
        };
        controls.Player.WASD.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    private void Update()
    {
        // Forward and backward movement
        Vector3 move = transform.forward * moveInput.y * moveSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        // Turning left and right
        float turn = moveInput.x * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }
}
