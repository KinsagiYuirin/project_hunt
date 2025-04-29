using MadDuck.Scripts.Character.Module;
using UnityEngine;

public class FootstepSoundPlayer : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private Vector2 volumeRange = new Vector2(0.6f, 1f);

    [Header("Dependencies")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private CharacterMovementModule objectRb;

    private float stepTimer = 0f;

    private void Update()
    {
        if (objectRb.MoveDirection.magnitude > 0.1f) // ถ้า Player กำลังเคลื่อนไหว
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // รีเซ็ตถ้าหยุดเดิน
        }
    }

    private void PlayFootstep()
    {
        if (footstepClips.Length == 0 || audioSource == null) return;

        var clip = footstepClips[Random.Range(0, footstepClips.Length)];
        var volume = Random.Range(volumeRange.x, volumeRange.y);
        audioSource.PlayOneShot(clip, volume);
    }
}
