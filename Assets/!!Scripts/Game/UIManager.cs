using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class UIManager : MonoBehaviour
{
    public GameObject howToPlayCanvas;
    public GameObject winCanvas;
    public GameObject loseCanvas;

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
            howToPlayCanvas.SetActive(true);
        }
    }

    public void HideHowToPlay()
    {
        if (howToPlayCanvas != null)
        {
            howToPlayCanvas.SetActive(false);
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowWinCanvas()
    {
        if (winCanvas != null)
        {
            winCanvas.SetActive(true);
        }
    }

    public void ShowLoseCanvas()
    {
        if (loseCanvas != null)
        {
            loseCanvas.SetActive(true);
        }
    }
}
