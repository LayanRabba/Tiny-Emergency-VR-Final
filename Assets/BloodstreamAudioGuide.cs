using UnityEngine;
using System.Collections;

public class BloodstreamAudioGuide : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource narrationAudio;

    [Header("Audio Clips")]
    public AudioClip[] clips;

    [Header("Timing")]
    public float gapBetweenClips = 10f;

    void Start()
    {
        StartCoroutine(PlayAudioGuideLoop());
    }

    IEnumerator PlayAudioGuideLoop()
    {
        while (true)
        {
            for (int i = 0; i < clips.Length; i++)
            {
                if (clips[i] == null)
                    continue;

                narrationAudio.clip = clips[i];
                narrationAudio.Play();

                // ننتظر حتى ينتهي المقطع
                yield return new WaitWhile(() => narrationAudio.isPlaying);

                // ننتظر 10 ثواني قبل المقطع التالي
                yield return new WaitForSeconds(gapBetweenClips);
            }

            // بعد F9 يرجع تلقائيًا إلى F1
        }
    }
}