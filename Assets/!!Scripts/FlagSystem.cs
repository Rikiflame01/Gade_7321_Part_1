using UnityEngine;

public class FlagSystem : MonoBehaviour
{
    [SerializeField]
    private Transform blueFlagSpawnTransform;

    public Transform flagslot;
    private void OnEnable()
    {
        GameEventSystem.OnFlagReset += ResetFlag;
    }
    private void OnDisable()
    {
        GameEventSystem.OnFlagReset -= ResetFlag;
    }

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
        if (collision.gameObject.CompareTag("RedFlag"))
        {
            GameEventSystem.ResetFlag(collision.gameObject);
        }
    }

    private void CollectBlueFlag(GameObject blueFlag)
    {
        blueFlag.transform.SetParent(flagslot);
        blueFlag.transform.localPosition = Vector3.zero;
        blueFlag.transform.localRotation = Quaternion.identity;

    }

    private void ResetFlag(GameObject flag)
    {
        if (flag.tag == "BlueFlag")
        {
            flag.transform.SetParent(null);
            flag.transform.position = blueFlagSpawnTransform.position;
        }
    }

}
