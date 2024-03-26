using UnityEngine;

// This class is used to create a visual effect when a bullet collides with a player or AI.

public class BulletCollisionVFX : MonoBehaviour
{
    [SerializeField] private GameObject collisionVFXPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("AI"))
        {
            GameObject vfx = Instantiate(collisionVFXPrefab, collision.contacts[0].point, Quaternion.identity);

            vfx.transform.localScale *= 5;

        }
    }
}
