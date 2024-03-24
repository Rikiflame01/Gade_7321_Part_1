using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public Transform shootingPoint;
    public Camera playerCamera; 

    private PlayerControls playerInput;
    private InputAction shootAction;

    private void Awake()
    {
        playerInput = new PlayerControls();
    }

    private void OnEnable()
    {
        shootAction = playerInput.Shoot.Shoot;
        shootAction.Enable();
        shootAction.performed += OnShoot;
    }

    private void OnDisable()
    {
        shootAction.Disable();
        shootAction.performed -= OnShoot;
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        if (projectilePrefab && shootingPoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            if (projectileRigidbody != null)
            {
                Vector3 shootingDirection = playerCamera.transform.forward;
                projectileRigidbody.AddForce(shootingDirection * 1000); 
            }
        }
    }
}
