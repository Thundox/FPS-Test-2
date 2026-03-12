using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour
{
    [Header("Intensity")]
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;

    [Header("Flicker Speed")]
    public float minFlickerTime = 0.05f;
    public float maxFlickerTime = 0.2f;

    private Light _light;
    private float _targetIntensity;
    private float _timer;

    void Awake()
    {
        _light = GetComponent<Light>();
        SetNewTarget();
    }

    void Update()
    {
        if (GetComponent<Light>() == null) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            SetNewTarget();
            Debug.Log("Set intensity to: " + _light.intensity);  // Watch Console
        }
    }

    void SetNewTarget()
    {
        _targetIntensity = Random.Range(minIntensity, maxIntensity);
        _timer = Random.Range(minFlickerTime, maxFlickerTime);
    }
}