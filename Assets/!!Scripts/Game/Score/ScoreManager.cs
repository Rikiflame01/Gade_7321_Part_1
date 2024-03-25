using Unity.VisualScripting;
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
            if (playerScore < 5)
            {
                playerScore++;
                if (playerScore == 5)
                {
                    GameEventSystem.PlayerWin();
                }
            }

        }
        else if (scorer.CompareTag("AI"))
        {
            if (aiScore < 5)
            {
                aiScore++;
                if (aiScore == 5)
                {
                    GameEventSystem.AIWin();
                }
            }

            
        }

        // Directly trigger the static events to update the score and reset the round
        GameEventSystem.ScoreUpdated(playerScore, aiScore);
        GameEventSystem.RoundReset();
    }
}
