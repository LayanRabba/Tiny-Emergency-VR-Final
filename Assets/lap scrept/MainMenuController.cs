using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject mainMenuCanvas;

    public void StartGame()
    {
        mainMenuCanvas.SetActive(false);
    }
}