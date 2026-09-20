using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject settingsPanel;

    public GameObject pauseButton;
    public GameObject resumeButton;

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);

        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}