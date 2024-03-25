using UnityEngine;

/* the flagsystem class is used to manage the flags in the game. 
 * * It is used to check for collision with the blue flag, collect the blue flag, 
 * * reset the flag, and check for collision with the red flag.
 * * */

public class FlagSystem : MonoBehaviour
{
    #region Dependencies
    [SerializeField]
    private Transform blueFlagSpawnTransform;

    public Transform flagslot;
    #endregion

    #region Unity Methods
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
    #endregion

    #region Private Methods
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
    #endregion
}
