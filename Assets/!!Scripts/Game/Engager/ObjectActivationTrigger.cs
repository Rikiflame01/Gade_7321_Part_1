using System.Collections;
using UnityEngine;

/* This class is used to activate objects when both the player and AI enter the trigger.
 * * It is used to activate objects after a delay and deactivate them after a duration.
 * * */
//This class's purpose is to force the player and AI to enage in 1v1 battles.

public class ObjectActivationTrigger : MonoBehaviour
{
    #region Dependencies
    public GameObject[] objectsToActivate;
    public float activationDelay = 1.5f;
    public float activeDuration = 15f;

    private bool playerInside = false;
    private bool aiInside = false;
    private Coroutine activationCoroutine;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter(Collider other)
    {
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
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
        else if (other.CompareTag("AI"))
        {
            aiInside = false;
        }

        if (!playerInside || !aiInside)
        {
            if (activationCoroutine != null)
            {
                StopCoroutine(activationCoroutine);
                activationCoroutine = null;
            }
            DeactivateObjects(); 
        }
    }
    #endregion

    #region Private Methods
    private void CheckAndStartActivation()
    {
        if (playerInside && aiInside && activationCoroutine == null)
        {
            activationCoroutine = StartCoroutine(ActivateObjectsAfterDelay());
        }
    }

    private IEnumerator ActivateObjectsAfterDelay()
    {
        yield return new WaitForSeconds(activationDelay);

        foreach (GameObject obj in objectsToActivate)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        yield return new WaitForSeconds(activeDuration);

        DeactivateObjects();

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
    #endregion
}
