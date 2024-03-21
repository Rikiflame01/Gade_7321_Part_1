using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int playerScore = 0;
    private int aiScore = 0;

    void OnEnable()
    {
        GameEventSystem.OnFlagCaptured += HandleFlagCapture;
    }

    void OnDisable()
    {
        GameEventSystem.OnFlagCaptured -= HandleFlagCapture;
    }

    private void HandleFlagCapture(GameObject scorer, string scoreType)
    {
        // Increment score based on who captured the flag
        if (scorer.CompareTag("Player"))
        {
            playerScore++;
        }
        else if (scorer.CompareTag("AI"))
        {
            aiScore++;
        }

        Debug.Log($"Score Updated. Player: {playerScore}, AI: {aiScore}");

        // Directly trigger the static events to update the score and reset the round
        GameEventSystem.ScoreUpdated(playerScore, aiScore);
        GameEventSystem.RoundReset();
    }
}
