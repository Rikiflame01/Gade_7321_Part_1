using UnityEngine;
using UnityEngine.InputSystem;

// This class is used to manage the player's movement ability.

public class PlayerMovement : MonoBehaviour
{
    #region Dependencies
    public float moveSpeed = 5f;
    public float turnSpeed = 200f; 
    private PlayerControls controls;
    private Vector2 moveInput;
    #endregion

    #region Unity Methods
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
    #endregion
}
