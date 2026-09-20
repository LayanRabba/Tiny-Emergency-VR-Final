using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject resumeButton;

    public void PauseGame()
    {
        Time.timeScale = 0f;

        pauseButton.SetActive(false);
        resumeButton.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;

        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }
}