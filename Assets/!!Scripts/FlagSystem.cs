using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FlagSystem : MonoBehaviour
{
    public Transform RedFlagSpawnPoint;
    public Transform BlueFlagSpawnPoint;

    private void OnCollisionEnter(Collision collision)
    {
        // Access the GameObject that was collided with
        GameObject otherGameObject = collision.gameObject;

        if (gameObject.CompareTag("Player"))
        {
            if (otherGameObject.CompareTag("BlueFlag"))
            {
                // Handle interaction with the Blue Flag
                FlagInteractionBlue(otherGameObject);
            }
            if (otherGameObject.CompareTag("RedFlag"))
            {
                // Assuming you have a method to return the Red Flag to its spawn
                ReturnFlagToSpawnRed(otherGameObject);
            }
        }
        else if (gameObject.tag == "AI")
        {
            if (otherGameObject.CompareTag("RedFlag"))
            {
                // Handle interaction with the Red Flag
                FlagInteractionRed(otherGameObject);
            }
            if (otherGameObject.CompareTag("BlueFlag"))
            {
                // Assuming you have a method to return the Blue Flag to its spawn
                ReturnFlagToSpawnBlue(otherGameObject);
            }
        }
    }


    void FlagInteractionRed(GameObject flag)
    {
        Transform flagSlot = transform.Find("FlagSlot");
        if (flagSlot != null)
        {
            // Teleport the flag to the FlagSlot
            flag.transform.position = flagSlot.position;
            flag.transform.parent = flagSlot;
        }
        else
        {
            Debug.LogError("FlagSlot not found");
        }
    }
    void FlagInteractionBlue(GameObject flag)
    {
        Transform flagSlot = transform.Find("FlagSlot");
        if (flagSlot != null)
        {
            // Teleport the flag to the FlagSlot
            flag.transform.position = flagSlot.position;
            flag.transform.parent = flagSlot;
        }
        else
        {
            Debug.LogError("FlagSlot not found");
        }
    }

    public void ReturnFlagToSpawnRed(GameObject flag)
    {
        // Resets the flag's parent (if it has one) and teleports it to its spawn point
        flag.transform.parent = null;
        flag.transform.position = RedFlagSpawnPoint.position;
    }
    public void ReturnFlagToSpawnBlue(GameObject flag)
    {
        // Resets the flag's parent (if it has one) and teleports it to its spawn point
        flag.transform.parent = null;
        flag.transform.position = BlueFlagSpawnPoint.position;
    }
}
