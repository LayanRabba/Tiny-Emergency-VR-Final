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

    private readonly List<Transform> waypoints = new();

    private int currentWaypointIndex = 1;
    private Vector3 pathCenter;
    private Vector2 playerOffset;

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

        // Place the vehicle at the first waypoint.
        pathCenter = waypoints[0].position;
        transform.position = pathCenter;

        // Point the vehicle toward the second waypoint.
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
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
            return;

        MoveForward(keyboard);
        MoveInsideVessel(keyboard);
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

        // Load the waypoint children in their Hierarchy order.
        foreach (Transform child in waypointsRoot)
        {
            waypoints.Add(child);
        }
    }

    void MoveForward(Keyboard keyboard)
    {
        // Forward: W or Up Arrow.
        bool forwardPressed =
            keyboard.wKey.isPressed ||
            keyboard.upArrowKey.isPressed;

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

        // Continue to the following waypoint.
        if (Vector3.Distance(
                pathCenter,
                targetPosition
            ) < 0.1f)
        {
            pathCenter = targetPosition;
            currentWaypointIndex++;
        }
    }

    void MoveInsideVessel(Keyboard keyboard)
    {
        float horizontalInput = 0f;
        float verticalInput = 0f;

        // Left: A or Left Arrow.
        if (
            keyboard.aKey.isPressed ||
            keyboard.leftArrowKey.isPressed)
        {
            horizontalInput = -1f;
        }
        // Right: D or Right Arrow.
        else if (
            keyboard.dKey.isPressed ||
            keyboard.rightArrowKey.isPressed)
        {
            horizontalInput = 1f;
        }

        // Up: E, Space, or Page Up.
        if (
            keyboard.eKey.isPressed ||
            keyboard.spaceKey.isPressed ||
            keyboard.pageUpKey.isPressed)
        {
            verticalInput = 1f;
        }
        // Down: Q, S, Down Arrow, or Page Down.
        else if (
            keyboard.qKey.isPressed ||
            keyboard.sKey.isPressed ||
            keyboard.downArrowKey.isPressed ||
            keyboard.pageDownKey.isPressed)
        {
            verticalInput = -1f;
        }

        playerOffset.x +=
            horizontalInput *
            sideSpeed *
            Time.deltaTime;

        playerOffset.y +=
            verticalInput *
            verticalSpeed *
            Time.deltaTime;

        /*
         * Keep the vehicle inside a circular boundary
         * around the center of the vessel.
         */
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