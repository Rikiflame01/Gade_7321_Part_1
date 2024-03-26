using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;

    [SerializeField] private AudioClip buttonClickSFX;

    // This script is responsible for pausing the game and displaying the pause menu.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            try
            {
                AudioManager.Instance.PlaySFX(buttonClickSFX, 3);
            }
            catch (System.Exception e)
            {
                Debug.Log("Sound Did not execute: " + e);
            }
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

    }
}