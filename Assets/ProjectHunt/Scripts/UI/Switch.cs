    using System;
using TriInspector;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private bool isOff;
    [SerializeField] private CameraZoom cameraZoom;
    [SerializeField,DisplayAsString] private bool isStart;
    public bool IsStart => isStart;

    private void Start()
    {
        isStart = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isStart = true;

            cameraZoom.SwitchCamera(isOff);
        }
    }
}
