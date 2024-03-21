using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private PlayerControls controls;
    private Vector2 moveInput;

    private void Awake()
    {
        // Initialize the controls
        controls = new PlayerControls();

        // Bind the movement action
        controls.Player.WASD.performed += ctx =>
        {
            // Ensure moveInput and other operations are valid here.
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
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(move);
    }
}
