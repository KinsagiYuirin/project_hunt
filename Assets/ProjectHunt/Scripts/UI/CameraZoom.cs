using TriInspector;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera zoomCamera;
    public CinemachineVirtualCamera farCamera;
    
    [SerializeField] private float activeDistance = 5f;
    [SerializeField, DisplayAsString] private bool zoomZoomActive;
    //public bool ZoomActive { get => zoomZoomActive; set => zoomZoomActive = value; }

    private void Start()
    {        
        zoomCamera.gameObject.SetActive(true);
        farCamera.gameObject.SetActive(false);
    }

    public void SwitchCamera(bool zoom)
    {
        zoomCamera.gameObject.SetActive(zoom);
        farCamera.gameObject.SetActive(!zoom);
    }
}