using TMPro;
using UnityEngine;

public class ScoreCanvas : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText;
    public TextMeshProUGUI aiScoreText;

    void OnEnable()
    {
        GameEventSystem.OnScoreCanvasUpdated += UpdateScoreUI;
    }

    void OnDisable()
    {
        GameEventSystem.OnScoreCanvasUpdated -= UpdateScoreUI;
    }

    private void UpdateScoreUI(int playerScore, int aiScore)
    {
        playerScoreText.text = "Player Score: " + playerScore.ToString();
        aiScoreText.text = "AI Score: " + aiScore.ToString();
        Debug.Log($"Updating score UI. Player: {playerScore}, AI: {aiScore}");
    }
}
