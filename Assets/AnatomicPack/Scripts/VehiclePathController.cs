
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VehiclePathController : MonoBehaviour
{
    [Header("Path")]
    public Transform waypointsRoot;

    [Header("Movement Speed")]
    public float forwardSpeed = 2f;
    public float sideSpeed = 1.5f;
    public float verticalSpeed = 1.5f;
    public float rotationSpeed = 5f;

    [Header("Vessel Boundary")]
    [Tooltip("Maximum distance from the center of the vessel")]
    public float maxOffsetRadius = 1.2f;

    [Header("XR Controller Input Actions")]
    [Tooltip("Left controller joystick - Vector2")]
    public InputActionReference moveAction;

    [Tooltip("Right controller trigger - Button")]
    public InputActionReference forwardAction;

    private readonly List<Transform> waypoints = new();

    private int currentWaypointIndex = 1;
    private Vector3 pathCenter;
    private Vector2 playerOffset;

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Enable();

        if (forwardAction != null && forwardAction.action != null)
            forwardAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
            moveAction.action.Disable();

        if (forwardAction != null && forwardAction.action != null)
            forwardAction.action.Disable();
    }

    void Start()
    {
        LoadWaypoints();

        if (waypoints.Count < 2)
        {
            Debug.LogError(
                "VehiclePathController needs at least two waypoints."
            );

            enabled = false;
            return;
        }

        pathCenter = waypoints[0].position;
        transform.position = pathCenter;

        Vector3 firstDirection =
            waypoints[1].position - waypoints[0].position;

        if (firstDirection.sqrMagnitude > 0.001f)
        {
            transform.rotation = Quaternion.LookRotation(
                firstDirection.normalized,
                Vector3.up
            );
        }
    }

    void Update()
    {
        MoveForward();
        MoveInsideVessel();
        ApplyFinalPosition();
    }

    void LoadWaypoints()
    {
        waypoints.Clear();

        if (waypointsRoot == null)
        {
            Debug.LogError("Waypoints Root is not assigned.");
            return;
        }

        foreach (Transform child in waypointsRoot)
        {
            waypoints.Add(child);
        }
    }

    void MoveForward()
    {
        if (forwardAction == null ||
            forwardAction.action == null)
            return;

        bool forwardPressed =
            forwardAction.action.IsPressed();

        if (!forwardPressed)
            return;

        if (currentWaypointIndex >= waypoints.Count)
            return;

        Vector3 targetPosition =
            waypoints[currentWaypointIndex].position;

        pathCenter = Vector3.MoveTowards(
            pathCenter,
            targetPosition,
            forwardSpeed * Time.deltaTime
        );

        Vector3 direction = targetPosition - pathCenter;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation =
                Quaternion.LookRotation(
                    direction.normalized,
                    Vector3.up
                );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        if (Vector3.Distance(
                pathCenter,
                targetPosition
            ) < 0.1f)
        {
            pathCenter = targetPosition;
            currentWaypointIndex++;
        }
    }

    void MoveInsideVessel()
    {
        if (moveAction == null ||
            moveAction.action == null)
            return;

        Vector2 joystickInput =
            moveAction.action.ReadValue<Vector2>();

        playerOffset.x +=
            joystickInput.x *
            sideSpeed *
            Time.deltaTime;

        playerOffset.y +=
            joystickInput.y *
            verticalSpeed *
            Time.deltaTime;

        playerOffset = Vector2.ClampMagnitude(
            playerOffset,
            maxOffsetRadius
        );
    }

    void ApplyFinalPosition()
    {
        Vector3 horizontalMovement =
            transform.right * playerOffset.x;

        Vector3 verticalMovement =
            transform.up * playerOffset.y;

        transform.position =
            pathCenter +
            horizontalMovement +
            verticalMovement;
    }
}
