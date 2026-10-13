
using UnityEngine;
using UnityEngine.InputSystem;

public class EndGame360Controller : MonoBehaviour
{
    public Material win360;
    public Material lose360;

    [Header("Objects to hide when game ends")]
    public GameObject[] objectsToHide;

    [Header("End Screen Buttons")]
    public GameObject win360Canvas;

    [Header("Player Camera")]
    public Camera playerCamera;

    private Vector3 startingCameraPosition;
    private Vector3 startingCanvasPosition;
    private bool endScreenActive = false;

    private void Start()
    {
        if (playerCamera == null)
            playerCamera = Camera.main;

        if (playerCamera != null && win360Canvas != null)
        {
            startingCameraPosition =
                playerCamera.transform.position;

            startingCanvasPosition =
                win360Canvas.transform.position;
        }

        if (win360Canvas != null)
            win360Canvas.SetActive(false);
    }

    private void Update()
    {
        if (Keyboard.current != null)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
                ShowWin360();

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
                ShowLose360();
        }
    }

    private void LateUpdate()
    {
        if (!endScreenActive ||
            playerCamera == null ||
            win360Canvas == null)
            return;

        // Follow camera POSITION only, not rotation.
        win360Canvas.transform.position =
            startingCanvasPosition +
            (playerCamera.transform.position -
             startingCameraPosition);
    }

    public void ShowWin360()
    {
        ShowEndScreen(win360);
    }

    public void ShowLose360()
    {
        ShowEndScreen(lose360);
    }

    private void ShowEndScreen(Material material)
    {
        HideGameplayObjects();

        if (material != null)
            RenderSettings.skybox = material;

        endScreenActive = true;

        if (win360Canvas != null)
        {
            if (playerCamera != null)
            {
                win360Canvas.transform.position =
                    startingCanvasPosition +
                    (playerCamera.transform.position -
                     startingCameraPosition);
            }

            win360Canvas.SetActive(true);
        }
    }

    private void HideGameplayObjects()
    {
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }
    }
}
