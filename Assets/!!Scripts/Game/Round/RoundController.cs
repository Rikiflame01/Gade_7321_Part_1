using UnityEngine;
using UnityEngine.AI;
using TMPro;
using System.Collections;
using Unity.VisualScripting;

/* 
 * * * This class is used to manage the round in the game.
 * * * It is used to manage the countdown to start the round, 
 * * * reset the character positions, update the score, reset the flag, and handle the win conditions.
 * * */
public class RoundController : MonoBehaviour
{

    #region Dependencies
    public GameObject WinCanvas;
    public GameObject LoseCanvas;

    public Transform playerStartTransform;
    public Transform aiStartTransform;

    public int countdownTime = 3;
    public TextMeshProUGUI countdownDisplay;

    public int currentRound = 1;
    public TextMeshProUGUI currentRoundDisplay;

    public Transform RedFlagSpawn;
    public Transform BlueFlagSpawn;

    public GameObject RedFlag;
    public GameObject BlueFlag;

    public GameObject player;
    public GameObject ai;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        ai.GetComponent<AIBehavior>().enabled = false;
        StartCoroutine(CountdownToStart());
    }

    void OnEnable()
    {
        GameEventSystem.OnCharacterReset += ResetCharacterPosition;
        GameEventSystem.OnRoundReset += ResetRoundPositions;
        GameEventSystem.OnScoreUpdated += UpdateScoreDisplay;
        GameEventSystem.OnAIWin += AIWins;
        GameEventSystem.OnPlayerWin += PlayerWins;
        GameEventSystem.OnFlagReset += ResetFlag;
    }

    void OnDisable()
    {
        GameEventSystem.OnCharacterReset -= ResetCharacterPosition;
        GameEventSystem.OnRoundReset -= ResetRoundPositions;
        GameEventSystem.OnScoreUpdated -= UpdateScoreDisplay;
        GameEventSystem.OnAIWin -= AIWins;
        GameEventSystem.OnPlayerWin -= PlayerWins;
        GameEventSystem.OnFlagReset -= ResetFlag;
    }
    #endregion

    #region Private Methods
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
        currentRound++;
        currentRoundDisplay.text = currentRound.ToString();

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

        player.GetComponent<PlayerMovement>().enabled = true;
        ai.GetComponent<AIBehavior>().enabled = true;

        yield return new WaitForSeconds(1f);
        countdownDisplay.gameObject.SetActive(false);
    }

    private void ResetCharacterPosition(GameObject character) 
    {
        if (character.tag == "AI")
        {
            RedFlag.transform.SetParent(null);
            ai.GetComponent<NavMeshAgent>().Warp(aiStartTransform.position);
        }
        if (character.tag == "Player")
        {
            BlueFlag.transform.SetParent(null);
            player.GetComponent<NavMeshAgent>().Warp(playerStartTransform.position);
        }

           
    }

    private void ResetFlag(GameObject flag)
    {
        if (flag.tag == "RedFlag")
        {
            flag.transform.SetParent(null);
            flag.transform.position = RedFlagSpawn.position;
        }
        if (flag.tag == "BlueFlag")
        {
            flag.transform.SetParent(null);
            flag.transform.position = BlueFlagSpawn.position;
        }
    }

    private void AIWins() 
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        ai.GetComponent<AIBehavior>().enabled = false;
        LoseCanvas.SetActive(true);

    }

    private void PlayerWins()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        ai.GetComponent<AIBehavior>().enabled = false;
        WinCanvas.SetActive(true);
        
    }
    #endregion
}

