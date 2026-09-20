using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class LaserShooter : MonoBehaviour
{
    [Header("Aim")]
    [SerializeField] private Camera aimCamera;

    [Header("Laser Origin")]
    [SerializeField] private Transform laserOrigin;

    [Header("Ignore")]
    [SerializeField] private Transform vehicleRoot;

    [Header("Laser Settings")]
    [SerializeField] private float range = 100f;

    [Header("Laser Appearance")]
    [SerializeField] private Color laserColor = Color.green;
    [SerializeField] private float laserWidth = 0.02f;

    private LineRenderer lineRenderer;

    private void Awake()
    {
        if (aimCamera == null)
            aimCamera = Camera.main;

        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
            lineRenderer = gameObject.AddComponent<LineRenderer>();

        ConfigureLine();
    }

    private void ConfigureLine()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;

        lineRenderer.startColor = laserColor;
        lineRenderer.endColor = laserColor;

        Shader shader =
            Shader.Find("Universal Render Pipeline/Unlit");

        if (shader == null)
            shader = Shader.Find("Unlit/Color");

        if (shader == null)
        {
            Debug.LogError("Laser shader not found.");
            return;
        }

        Material material = new Material(shader);

        material.color = laserColor;

        if (material.HasProperty("_BaseColor"))
            material.SetColor("_BaseColor", laserColor);

        lineRenderer.material = material;
    }

    private void Update()
    {
        if (aimCamera == null)
            aimCamera = Camera.main;

        if (IsFireHeld())
        {
            FireLaser();
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }

    private bool IsFireHeld()
    {
#if ENABLE_INPUT_SYSTEM
        return Keyboard.current != null &&
               Keyboard.current.fKey.isPressed;
#else
        return Input.GetKey(KeyCode.F);
#endif
    }

    private void FireLaser()
    {
        if (aimCamera == null || laserOrigin == null)
            return;

        lineRenderer.enabled = true;

        Vector3 start = laserOrigin.position;

        Vector3 direction =
            aimCamera.transform.forward;

        Vector3 end =
            start + direction * range;

        RaycastHit[] hits =
            Physics.RaycastAll(
                start,
                direction,
                range,
                Physics.DefaultRaycastLayers,
                QueryTriggerInteraction.Collide
            );

        RaycastHit closestHit = default;

        float closestDistance =
            Mathf.Infinity;

        bool foundHit = false;

        foreach (RaycastHit hit in hits)
        {
            Transform hitTransform =
                hit.collider.transform;

            // تجاهل المركبة وأجزائها
            if (
                vehicleRoot != null &&
                (
                    hitTransform == vehicleRoot ||
                    hitTransform.IsChildOf(vehicleRoot)
                )
            )
            {
                continue;
            }

            if (hit.distance < closestDistance)
            {
                closestDistance =
                    hit.distance;

                closestHit = hit;

                foundHit = true;
            }
        }

        if (foundHit)
        {
            end = closestHit.point;

            ClotHealth clot =
                closestHit.collider
                .GetComponentInParent<ClotHealth>();

            if (clot != null)
            {
                clot.HitByLaser();
            }
        }

        lineRenderer.SetPosition(
            0,
            start
        );

        lineRenderer.SetPosition(
            1,
            end
        );
    }
}