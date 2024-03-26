using Unity.VisualScripting;
using UnityEngine;

// This class is used to manage the score in the game.

public class ScoreManager : MonoBehaviour
{
    private int playerScore = 0;
    private int aiScore = 0;

    [SerializeField] private AudioClip AIWinsSFX;
    [SerializeField] private AudioClip PlayerWinsSFX;

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
        if (scorer.CompareTag("Player"))
        {
            if (playerScore < 5)
            {
                playerScore++;
                AudioManager.Instance.PlaySFX(PlayerWinsSFX, 3);
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
                AudioManager.Instance.PlaySFX(AIWinsSFX, 3);
                if (aiScore == 5)
                {
                    GameEventSystem.AIWin();
                }
            }

            
        };
        GameEventSystem.ScoreUpdated(playerScore, aiScore);
        GameEventSystem.RoundReset();
    }
}
