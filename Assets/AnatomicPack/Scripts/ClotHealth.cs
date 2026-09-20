using UnityEngine;
using System.Collections;

public class ClotHealth : MonoBehaviour
{
    [Header("Win UI")]
    public GameObject winCanvas;

    [Header("Particles")]
    public ParticleSystem clotDebris;

    [Header("Audio")]
    public AudioSource clotBreakAudio;

    [Header("Timing")]
    public float waitBeforeBreak = 1f;
    public float breakDuration = 2f;
    public float winScreenDelay = 1.5f;

    [Header("Break Effect")]
    public float explosionForce = 1.5f;
    public float shrinkSpeed = 2f;

    private bool hit = false;

    private void Awake()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);

        if (clotDebris != null)
            clotDebris.Stop(
                true,
                ParticleSystemStopBehavior.StopEmittingAndClear
            );

        if (clotBreakAudio != null)
        {
            clotBreakAudio.playOnAwake = false;
            clotBreakAudio.loop = false;
        }
    }

    public void HitByLaser()
    {
        if (hit)
            return;

        hit = true;

        Debug.Log("Clot hit by laser!");

        StartCoroutine(BreakClotSequence());
    }

    private IEnumerator BreakClotSequence()
    {
        // استنى شوي بعد إصابة الليزر
        yield return new WaitForSeconds(waitBeforeBreak);

        Collider clotCollider = GetComponent<Collider>();

        if (clotCollider != null)
            clotCollider.enabled = false;

        Transform[] pieces = GetComponentsInChildren<Transform>();

        float timer = 0f;
        bool particlesStarted = false;

        while (timer < breakDuration)
        {
            timer += Time.deltaTime;

            // بداية التفتت + الفتافيت + الصوت
            if (!particlesStarted && timer >= 0.15f)
            {
                particlesStarted = true;

                // شغل الفتافيت
                if (clotDebris != null)
                {
                    var main = clotDebris.main;

                    main.startSize = 0.15f;
                    main.startSpeed = 1.2f;
                    main.startLifetime = 1.5f;

                    clotDebris.Play();
                }

                // شغل صوت تدمير التجلط
                if (clotBreakAudio != null)
                {
                    clotBreakAudio.Play();
                }
            }

            // حركة قطع التجلط
            foreach (Transform piece in pieces)
            {
                if (piece == transform)
                    continue;

                // لا نحرك الـParticle System
                if (clotDebris != null &&
                    piece == clotDebris.transform)
                    continue;

                Vector3 direction =
                    (piece.position - transform.position).normalized;

                piece.position +=
                    direction *
                    explosionForce *
                    Time.deltaTime;

                piece.localScale =
                    Vector3.Lerp(
                        piece.localScale,
                        Vector3.zero,
                        shrinkSpeed * Time.deltaTime
                    );
            }

            yield return null;
        }

        // اخفاء قطع التجلط
        foreach (Transform piece in pieces)
        {
            if (piece == transform)
                continue;

            // لا نخفي الـparticles مباشرة
            if (clotDebris != null &&
                piece == clotDebris.transform)
                continue;

            piece.gameObject.SetActive(false);
        }

        Debug.Log("Blood clot cleared!");

        // استنى قبل شاشة الفوز
        yield return new WaitForSeconds(winScreenDelay);

        if (winCanvas != null)
            winCanvas.SetActive(true);

        Debug.Log("MISSION COMPLETE!");
    }
}