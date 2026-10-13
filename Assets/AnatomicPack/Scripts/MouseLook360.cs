using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook360 : MonoBehaviour
{
    public float sensitivity = 0.2f;

    private float yaw;
    private float pitch;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.rightButton.isPressed)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();

            yaw += delta.x * sensitivity;
            pitch -= delta.y * sensitivity;

            pitch = Mathf.Clamp(pitch, -89f, 89f);

            transform.localRotation =
                Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}