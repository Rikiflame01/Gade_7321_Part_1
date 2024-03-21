using UnityEngine;

public class FlagSystem : MonoBehaviour
{
    [SerializeField]
    private Transform blueFlagSpawnPoint; // Assign in the Inspector
    public Transform flagslot;

    private void OnCollisionEnter(Collision collision)
    {
        // Check for collision with the Blue Flag
        if (collision.gameObject.CompareTag("BlueFlag"))
        {
            CollectBlueFlag(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("PlayerSpawnPoint") && flagslot.childCount > 0)
        {
            GameEventSystem.FlagCaptured(gameObject, "collected blue flag");
            GameEventSystem.RoundReset();
        }
    }

    private void CollectBlueFlag(GameObject blueFlag)
    {
        blueFlag.transform.SetParent(flagslot);
        blueFlag.transform.localPosition = Vector3.zero;
        blueFlag.transform.localRotation = Quaternion.identity;

    }

}
