using UnityEngine;
using System.Collections;

public class ClotHealth : MonoBehaviour
{
    [Header("Win UI")]
    public GameObject winCanvas;

    [Header("Timing")]
    public float clotDisappearDelay = 8f;
    public float winScreenDelay = 1.5f;

    private bool hit = false;

    private void Awake()
    {
        if (winCanvas != null)
            winCanvas.SetActive(false);
    }

    public void HitByLaser()
    {
        if (hit)
            return;

        hit = true;

        Debug.Log("Clot hit by laser!");

        StartCoroutine(WinSequence());
    }

    private IEnumerator WinSequence()
    {
        // استنى شوي بعد إصابة التجلط
        yield return new WaitForSeconds(clotDisappearDelay);

        // اخفاء كل أجزاء التجلط
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        // تعطيل الكوليدر
        Collider clotCollider = GetComponent<Collider>();

        if (clotCollider != null)
            clotCollider.enabled = false;

        Debug.Log("Blood clot cleared!");

        // استنى شوي قبل شاشة الفوز
        yield return new WaitForSeconds(winScreenDelay);

        // اظهار شاشة الفوز
        if (winCanvas != null)
            winCanvas.SetActive(true);

        Debug.Log("MISSION COMPLETE!");
    }
}