using TriInspector;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public CinemachineVirtualCamera virtualCamera2;
    
    [SerializeField] private float activeDistance = 5f;
    [SerializeField, DisplayAsString] private bool active;
    public bool Active { get => active; set => active = value; }

    private void Start()
    {
        active = false;
        virtualCamera.gameObject.SetActive(true);
        virtualCamera2.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (active) 
        {
            SwitchToFarCamera();
        }
        else
        {
            SwitchToNearCamera();
        }
    }

    private void SwitchToNearCamera()
    {
        // เปลี่ยนไปใช้กล้องใกล้
        virtualCamera.gameObject.SetActive(true);
        virtualCamera2.gameObject.SetActive(false);
    }

    private void SwitchToFarCamera()
    {
        // เปลี่ยนไปใช้กล้องไกล
        virtualCamera.gameObject.SetActive(false);
        virtualCamera2.gameObject.SetActive(true);
    }
}