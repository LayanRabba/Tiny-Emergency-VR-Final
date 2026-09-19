using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraWaypoint : MonoBehaviour
{
    [Header("Path")]
    public List<Transform> waypoints;
    public Transform MyTransform;

    [Header("Movement Speed")]
    public float forwardSpeed = 0.04f;
    public float sidewaysSpeed = 0.025f;
    public float verticalSpeed = 0.025f;
    public float rotationSpeed = 6f;

    [Header("Movement Limits Inside Vessel")]
    public float maxHorizontalOffset = 0.02f;
    public float maxVerticalOffset = 0.02f;

    private int currentWaypointIndex = 1;
    private Vector3 pathPosition;

    private float horizontalOffset;
    private float verticalOffset;

    void Start()
    {
        if (MyTransform == null)
            MyTransform = transform;

        if (waypoints == null || waypoints.Count < 2)
        {
            Debug.LogError("Please assign at least two waypoints.");
            enabled = false;
            return;
        }

        pathPosition = waypoints[0].position;
        MyTransform.position = pathPosition;

        LookTowardsNextWaypoint();
    }

    void Update()
    {
        HandleForwardMovement();
        HandleSideMovement();
        UpdateVehiclePosition();
    }

   void HandleForwardMovement()
{
    Keyboard keyboard = Keyboard.current;

    if (keyboard == null)
        return;

    bool moveForward =
        keyboard.wKey.isPressed ||
        keyboard.upArrowKey.isPressed;

    if (!moveForward || currentWaypointIndex >= waypoints.Count)
        return;

    Vector3 targetPosition = waypoints[currentWaypointIndex].position;

    pathPosition = Vector3.MoveTowards(
        pathPosition,
        targetPosition,
        forwardSpeed * Time.deltaTime
    );

    Vector3 direction = targetPosition - pathPosition;

    if (direction.sqrMagnitude > 0.000001f)
    {
        Quaternion targetRotation =
            Quaternion.LookRotation(direction.normalized, Vector3.up);

        MyTransform.rotation = Quaternion.Slerp(
            MyTransform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    if (Vector3.Distance(pathPosition, targetPosition) < 0.002f)
    {
        pathPosition = targetPosition;
        currentWaypointIndex++;
    }
}

void HandleSideMovement()
{
    Keyboard keyboard = Keyboard.current;

    if (keyboard == null)
        return;

    float horizontalInput = 0f;
    float verticalInput = 0f;

    if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        horizontalInput = -1f;

    if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        horizontalInput = 1f;

    if (keyboard.eKey.isPressed)
        verticalInput = 1f;

    if (keyboard.qKey.isPressed)
        verticalInput = -1f;

    horizontalOffset +=
        horizontalInput * sidewaysSpeed * Time.deltaTime;

    verticalOffset +=
        verticalInput * verticalSpeed * Time.deltaTime;

    horizontalOffset = Mathf.Clamp(
        horizontalOffset,
        -maxHorizontalOffset,
        maxHorizontalOffset
    );

    verticalOffset = Mathf.Clamp(
        verticalOffset,
        -maxVerticalOffset,
        maxVerticalOffset
    );
}

    void UpdateVehiclePosition()
    {
        Vector3 sideMovement =
            MyTransform.right * horizontalOffset;

        Vector3 verticalMovement =
            MyTransform.up * verticalOffset;

        MyTransform.position =
            pathPosition + sideMovement + verticalMovement;
    }

    void LookTowardsNextWaypoint()
    {
        Vector3 direction =
            waypoints[currentWaypointIndex].position - pathPosition;

        if (direction.sqrMagnitude > 0.000001f)
        {
            MyTransform.rotation =
                Quaternion.LookRotation(direction.normalized, Vector3.up);
        }
    }
}