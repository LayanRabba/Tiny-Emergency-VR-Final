using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    public AudioSource footstepsAudio;
    public CharacterController characterController;

    public float minimumSpeed = 0.1f;

    void Update()
    {
        Vector3 velocity = characterController.velocity;

        // نهتم بالحركة الأفقية فقط
        float movementSpeed = new Vector3(
            velocity.x,
            0f,
            velocity.z
        ).magnitude;

        if (movementSpeed > minimumSpeed)
        {
            if (!footstepsAudio.isPlaying)
                footstepsAudio.Play();
        }
        else
        {
            if (footstepsAudio.isPlaying)
                footstepsAudio.Stop();
        }
    }
}