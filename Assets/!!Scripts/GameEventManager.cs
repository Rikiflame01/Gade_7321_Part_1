using UnityEngine;

public static class GameEventSystem
{
    public delegate void ScoreCanvasUpdateHandler(int playerScore, int aiScore);
    public static event ScoreCanvasUpdateHandler OnScoreCanvasUpdated;

    public delegate void FlagCapturedHandler(GameObject scorer, string scoreType);
    public static event FlagCapturedHandler OnFlagCaptured;

    public delegate void RoundResetHandler();
    public static event RoundResetHandler OnRoundReset;

    public delegate void PlayerWinHandler();
    public static event PlayerWinHandler OnPlayerWin;

    public delegate void AIWinHandler();
    public static event  AIWinHandler OnAIWin;

    public delegate void FlagResetHandler(GameObject flag);
    public static event FlagResetHandler OnFlagReset;

    public delegate void CharacterResetHandler(GameObject flag);
    public static event CharacterResetHandler OnCharacterReset;

    public delegate void ScoreUpdateHandler(int playerScore, int aiScore);
    public static event ScoreUpdateHandler OnScoreUpdated;

    public static void FlagCaptured(GameObject scorer, string scoreType)
    {
        OnFlagCaptured?.Invoke(scorer, scoreType);
    }

    public static void RoundReset()
    {
        OnRoundReset?.Invoke();
    }

    public static void ScoreUpdated(int playerScore, int aiScore)
    {
        OnScoreUpdated?.Invoke(playerScore, aiScore);
    }

    public static void ScoreCanvasUpdated(int playerScore, int aiScore)
    {
        OnScoreCanvasUpdated?.Invoke(playerScore, aiScore);
    }

    public static void ResetFlag(GameObject flag) 
    { 
        OnFlagReset?.Invoke(flag);
    }

    public static void ResetCharacter(GameObject character)
    {
        OnCharacterReset?.Invoke(character);
    }

    public static void PlayerWin()
    {
        OnPlayerWin?.Invoke();
    }

    public static void AIWin()
    {
        OnAIWin?.Invoke();
    }
}
