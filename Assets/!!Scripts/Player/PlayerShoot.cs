using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootingPoint;
    public Camera playerCamera;

    private PlayerControls playerInput;
    private InputAction shootAction;
    private float shootCooldown = 5.0f; 
    private float nextShootTime = 0f; 

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
        if (Time.time >= nextShootTime)
        {
            ShootProjectile();
            nextShootTime = Time.time + shootCooldown;
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab && shootingPoint)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootingPoint.position, Quaternion.LookRotation(playerCamera.transform.forward));
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();

            if (projectileRigidbody != null)
            {
                Vector3 shootingDirection = playerCamera.transform.forward;
                projectileRigidbody.AddForce(shootingDirection * 500);
            }
        }
    }
}
