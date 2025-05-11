using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake Instance;

    [SerializeField] private Transform camTransform;
    [SerializeField] private float shakeDuration = 0.2f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private Vector3 initialPosition;
    private float shakeTimer = 0f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (camTransform == null)
            camTransform = Camera.main.transform;

        initialPosition = camTransform.localPosition;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            camTransform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            camTransform.localPosition = initialPosition;
        }
    }

    public void Shake(float duration = -1f, float magnitude = -1f)
    {
        shakeTimer = (duration > 0f) ? duration : shakeDuration;
        shakeMagnitude = (magnitude > 0f) ? magnitude : shakeMagnitude;
    }
}
