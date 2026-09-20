using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LaserShooter : MonoBehaviour
{
    [Header("Laser Setup")]
    public Camera aimCamera;
    public Transform laserOrigin;
    public Transform vehicleRoot;

    [Header("Laser Settings")]
    public float range = 100f;
    public Color laserColor = Color.green;
    public float laserWidth = 0.02f;

    [Header("Laser Sound")]
    public AudioSource laserAudio;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        // إنشاء Line Renderer لليزر
        lineRenderer = gameObject.GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        lineRenderer.material =
            new Material(Shader.Find("Sprites/Default"));

        lineRenderer.enabled = false;

        // تأكد إن الصوت مش شغال من البداية
        if (laserAudio != null)
        {
            laserAudio.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (IsFireHeld())
        {
            FireLaser();

            // شغّل صوت الليزر
            if (laserAudio != null && !laserAudio.isPlaying)
            {
                laserAudio.Play();
            }
        }
        else
        {
            lineRenderer.enabled = false;

            // وقف الصوت لما نترك F
            if (laserAudio != null && laserAudio.isPlaying)
            {
                laserAudio.Stop();
            }
        }
    }

    private bool IsFireHeld()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            return Keyboard.current.fKey.isPressed;
        }
#endif

        return Input.GetKey(KeyCode.F);
    }

    private void FireLaser()
    {
        if (aimCamera == null || laserOrigin == null)
            return;

        lineRenderer.enabled = true;

        Vector3 startPoint = laserOrigin.position;
        Vector3 direction = aimCamera.transform.forward;

        Vector3 endPoint =
            startPoint + direction * range;

        // افحص كل الأشياء اللي الليزر لمسها
        RaycastHit[] hits =
            Physics.RaycastAll(
                startPoint,
                direction,
                range
            );

        RaycastHit closestHit = new RaycastHit();
        bool foundHit = false;
        float closestDistance = Mathf.Infinity;

        foreach (RaycastHit hit in hits)
        {
            // تجاهل أجزاء المركبة
            if (vehicleRoot != null &&
                hit.collider.transform.IsChildOf(vehicleRoot))
            {
                continue;
            }

            float distance =
                Vector3.Distance(
                    startPoint,
                    hit.point
                );

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestHit = hit;
                foundHit = true;
            }
        }

        if (foundHit)
        {
            endPoint = closestHit.point;

            // افحص إذا ضرب التجلط
            ClotHealth clot =
                closestHit.collider
                .GetComponentInParent<ClotHealth>();

            if (clot != null)
            {
                clot.HitByLaser();
            }
        }

        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}