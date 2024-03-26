using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

// This class is used to manage the UI interactions in the game.

public class UIManager : MonoBehaviour
{
    public GameObject howToPlayCanvas;
    public GameObject winCanvas;
    public GameObject loseCanvas;
    [SerializeField] private AudioClip buttonClickSFX;

    private void Start()
    {
        SetActiveCanvas();
    }

    public void SetActiveCanvas()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "MainMenu")
        {
            if (howToPlayCanvas != null) howToPlayCanvas.SetActive(false);
            if (winCanvas != null) winCanvas.SetActive(false);
            if (loseCanvas != null) loseCanvas.SetActive(false);
        }
        else if (currentScene.name == "Level1")
        {
            if (winCanvas != null) winCanvas.SetActive(false);
            if (loseCanvas != null) loseCanvas.SetActive(false);
            if (howToPlayCanvas != null) howToPlayCanvas.SetActive(false);
        }
    }

    public void ShowHowToPlay()
    {
        if (howToPlayCanvas != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
            howToPlayCanvas.SetActive(true);
        }
    }

    public void HideHowToPlay()
    {
        if (howToPlayCanvas != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
            howToPlayCanvas.SetActive(false);
        }
    }

    public void PlayGame()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
        Application.Quit();
    }

    public void ShowWinCanvas()
    {
        if (winCanvas != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
            winCanvas.SetActive(true);
        }
    }

    public void ShowLoseCanvas()
    {
        if (loseCanvas != null)
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 2);
            loseCanvas.SetActive(true);
        }
    }
}
