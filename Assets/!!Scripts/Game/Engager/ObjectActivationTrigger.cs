using System.Collections;
using UnityEngine;

public class ObjectActivationTrigger : MonoBehaviour
{
    public GameObject[] objectsToActivate;
    public float activationDelay = 1.5f;
    public float activeDuration = 30f;

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

        // If either leaves, stop trying to activate the objects
        if (activationCoroutine != null)
        {
            StopCoroutine(activationCoroutine);
            activationCoroutine = null;
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
        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }

        // Reset the coroutine tracker
        activationCoroutine = null;
    }
}
