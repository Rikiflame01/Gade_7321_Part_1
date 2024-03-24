using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;

public class RoundController : MonoBehaviour
{
    public Transform playerStartTransform;
    public Transform aiStartTransform;

    public int countdownTime = 3;
    public TextMeshProUGUI countdownDisplay;

    public Transform RedFlagSpawn;
    public Transform BlueFlagSpawn;

    public GameObject RedFlag;
    public GameObject BlueFlag;

    public GameObject player;
    public GameObject ai;

    private void Awake()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        ai.GetComponent<AIBehavior>().enabled = false;
        StartCoroutine(CountdownToStart());
    }

    void OnEnable()
    {
        GameEventSystem.OnRoundReset += ResetRoundPositions;
        GameEventSystem.OnScoreUpdated += UpdateScoreDisplay;
    }

    void OnDisable()
    {
        GameEventSystem.OnRoundReset -= ResetRoundPositions;
        GameEventSystem.OnScoreUpdated -= UpdateScoreDisplay;
    }

    private void ResetRoundPositions()
    {
        player.GetComponent<NavMeshAgent>().Warp(playerStartTransform.position);
        ai.GetComponent<NavMeshAgent>().Warp(aiStartTransform.position);
        BlueFlag.transform.SetParent(null);
        RedFlag.transform.SetParent(null);
        ToggleFlagPhysics(RedFlag, false);
        ToggleFlagPhysics(BlueFlag, false);

        RedFlag.transform.position = RedFlagSpawn.position;
        BlueFlag.transform.position = BlueFlagSpawn.position;

        ToggleFlagPhysics(RedFlag, true);
        ToggleFlagPhysics(BlueFlag, true);
    }

    private void UpdateScoreDisplay(int playerScore, int aiScore)
    {
        // Directly trigger the static event to update UI elements with new scores
        GameEventSystem.ScoreCanvasUpdated(playerScore, aiScore);
    }
    private void ToggleFlagPhysics(GameObject flag, bool enablePhysics)
    {
        if (flag.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            if (!enablePhysics)
            {
                // Disable physics interactions by making Rigidbody kinematic
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            else
            {
                // Re-enable physics interactions
                rb.isKinematic = false;
            }
        }

    }

    public IEnumerator CountdownToStart()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();

            yield return new WaitForSeconds(1f);

            countdownTime--;
        }

        countdownDisplay.text = "GO!";

        // Enable player and AI movement here
        player.GetComponent<PlayerMovement>().enabled = true;
        ai.GetComponent<AIBehavior>().enabled = true;

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
    }
}

