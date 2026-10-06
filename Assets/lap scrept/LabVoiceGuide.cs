using UnityEngine;
using System.Collections;

public class LabVoiceGuide : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource voiceAudio;
    public AudioClip introClip;
    public AudioClip reminderClip;

    [Header("Timing")]
    public float firstReminderDelay = 10f;
    public float repeatReminderDelay = 10f;

    private bool guideStarted = false;
    private Coroutine audioRoutine;

    void Start()
    {
        // لا يبدأ الصوت قبل الضغط على START
        voiceAudio.loop = false;
        voiceAudio.Stop();
    }

    // يتم استدعاؤها من زر START
    public void StartGuide()
    {
        // منع تشغيله أكثر من مرة
        if (guideStarted)
            return;

        guideStarted = true;
        audioRoutine = StartCoroutine(PlayGuide());
    }

    IEnumerator PlayGuide()
    {
        // 1) المقدمة تعمل مرة واحدة كاملة
        if (introClip != null)
        {
            voiceAudio.clip = introClip;
            voiceAudio.Play();

            // ننتظر حتى ينتهي صوت المقدمة
            yield return new WaitWhile(() => voiceAudio.isPlaying);
        }

        // 2) انتظار 10 ثواني
        yield return new WaitForSeconds(firstReminderDelay);

        // 3) تكرار التذكير طالما نحن ما زلنا في هذا المشهد
        while (true)
        {
            if (reminderClip != null)
            {
                voiceAudio.clip = reminderClip;
                voiceAudio.Play();

                // ننتظر حتى ينتهي التذكير
                yield return new WaitWhile(() => voiceAudio.isPlaying);
            }

            yield return new WaitForSeconds(repeatReminderDelay);
        }
    }
}