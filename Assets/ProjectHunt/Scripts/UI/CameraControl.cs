using System;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private bool isOff;
    [SerializeField] private CameraZoom cameraZoom;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isOff)
            {
                cameraZoom.Active = true;
            }
            else
            {
                cameraZoom.Active = false;
            }
        }
    }
}
