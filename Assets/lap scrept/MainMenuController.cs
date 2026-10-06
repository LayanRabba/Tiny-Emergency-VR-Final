using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Start UI")]
    public GameObject startCanvas;

    [Header("Pause / Resume")]
    public GameObject pauseButton;
    public GameObject resumeButton;

    [Header("Voice Guide")]
    public LabVoiceGuide labVoiceGuide;

    public void StartGame()
    {
        // إخفاء شاشة البداية
        if (startCanvas != null)
            startCanvas.SetActive(false);

        // إظهار زر Pause
        if (pauseButton != null)
            pauseButton.SetActive(true);

        // إخفاء زر Resume بالبداية
        if (resumeButton != null)
            resumeButton.SetActive(false);

        // تشغيل الصوت الخاص ببداية المختبر
        if (labVoiceGuide != null)
            labVoiceGuide.StartGuide();

        // التأكد أن اللعبة غير متوقفة
        Time.timeScale = 1f;

        Debug.Log("GAME STARTED");
    }
}