using System.Collections;
using UnityEngine;

public class ObjectActivationTrigger : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public float activationDelay = 1.5f;
    public float activeDuration = 15f;

    private bool playerInside = false;
    private bool aiInside = false;
    private Coroutine activationCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player or AI enters the trigger
        if (other.CompareTag("Player"))
        {
            playerInside = true;
        }
        else if (other.CompareTag("AI"))
        {
            aiInside = true;
        }

        CheckAndStartActivation();
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the player or AI exits the trigger
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
        else if (other.CompareTag("AI"))
        {
            aiInside = false;
        }

        // If either leaves, consider deactivating the objects immediately or after a delay
        if (!playerInside || !aiInside)
        {
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
            }
            DeactivateObjects(); // Call DeactivateObjects to ensure objects are turned off if one leaves early
        }
    }

    private void CheckAndStartActivation()
    {
        // If both the player and AI are inside, start the activation process
        if (playerInside && aiInside && activationCoroutine == null)
        {
            activationCoroutine = StartCoroutine(ActivateObjectsAfterDelay());
        }
    }

    private IEnumerator ActivateObjectsAfterDelay()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(activationDelay);

        // Activate the objects
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        // Wait for the duration the objects should remain active
        yield return new WaitForSeconds(activeDuration);

        // Deactivate the objects
        DeactivateObjects();

        // Reset the coroutine tracker
        activationCoroutine = null;
    }

    private void DeactivateObjects()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
