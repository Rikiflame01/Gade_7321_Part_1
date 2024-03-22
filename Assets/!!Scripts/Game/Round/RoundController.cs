using UnityEngine;
using UnityEngine.AI;

public class RoundController : MonoBehaviour
{
    public Transform playerStartTransform;
    public Transform aiStartTransform;

    public Transform RedFlagSpawn;
    public Transform BlueFlagSpawn;

    public GameObject RedFlag;
    public GameObject BlueFlag;

    public GameObject player;
    public GameObject ai;

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
        Debug.Log("Round positions reset.");
    }

    private void UpdateScoreDisplay(int playerScore, int aiScore)
    {
        // Directly trigger the static event to update UI elements with new scores
        GameEventSystem.ScoreCanvasUpdated(playerScore, aiScore);
        Debug.Log($"Score display updated. Player: {playerScore}, AI: {aiScore}");
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
}
