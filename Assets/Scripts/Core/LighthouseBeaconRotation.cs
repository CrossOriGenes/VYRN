using UnityEngine;

public class LighthouseBeaconRotation : MonoBehaviour
{
    [Header("Beacon Rotation")]
    [SerializeField] private Transform _beaconRotator;
    [Range(1f, 60f)]
    [SerializeField] private float _rotationSpeed = 15f;


    private void Update()
    {
        if (_beaconRotator == null) return;

        _beaconRotator.Rotate(
            Vector3.forward,
            _rotationSpeed * Time.deltaTime,
            Space.Self
        );
    }
}
