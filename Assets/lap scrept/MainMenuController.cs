using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("Start UI")]
    public GameObject startCanvas;

    [Header("Clinic")]
    public GameObject clinicRoot;

    [Header("Pause / Resume")]
    public GameObject pauseButton;
    public GameObject resumeButton;

    [Header("Voice Guide")]
    public LabVoiceGuide labVoiceGuide;

    private Renderer[] clinicRenderers;

    private void Start()
    {
        Time.timeScale = 1f;

        // نجيب كل الأشياء المرئية داخل العيادة
        if (clinicRoot != null)
        {
            clinicRenderers =
                clinicRoot.GetComponentsInChildren<Renderer>(true);

            // نخفي الشكل فقط
            SetClinicVisible(false);
        }

        if (pauseButton != null)
            pauseButton.SetActive(false);

        if (resumeButton != null)
            resumeButton.SetActive(false);
    }

    public void StartGame()
    {
        // إظهار العيادة بصريًا
        SetClinicVisible(true);

        // إخفاء شاشة START
        if (startCanvas != null)
            startCanvas.SetActive(false);

        // إظهار Pause
        if (pauseButton != null)
            pauseButton.SetActive(true);

        // Resume مخفي بالبداية
        if (resumeButton != null)
            resumeButton.SetActive(false);

        // تشغيل الصوت
        if (labVoiceGuide != null)
            labVoiceGuide.StartGuide();

        Time.timeScale = 1f;

        Debug.Log("GAME STARTED");
    }

    private void SetClinicVisible(bool visible)
    {
        if (clinicRenderers == null)
            return;

        foreach (Renderer r in clinicRenderers)
        {
            if (r != null)
                r.enabled = visible;
        }
    }
}