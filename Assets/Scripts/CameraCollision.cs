using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _camera;
    [SerializeField] private float _returnSpeed = 4f;
    [SerializeField] private Transform _zoomObject;

    private Vector3 _cameraOffset;
    private float _maxCameraDistance;

    private void Awake()
    {
        _cameraOffset = _camera.localPosition;
        _maxCameraDistance = Vector3.Distance(transform.position, transform.position + _cameraOffset);
    }

    private void FixedUpdate()
    {
        float castRadius = 0.2f;
        Vector3 direction = -_camera.forward;
        if (Physics.SphereCast(transform.position, castRadius, direction, out var hit, _maxCameraDistance, _layerMask))
        {
            float distPercent = 1f - Mathf.Clamp01((hit.distance + castRadius / 2f) / _maxCameraDistance);
            float zoomY = Mathf.Lerp(_cameraOffset.y, _zoomObject.localPosition.y, distPercent);
            float zoomZ = Mathf.Lerp(_cameraOffset.z, _zoomObject.localPosition.z, distPercent);
            Vector3 pos = new(_cameraOffset.x, zoomY, zoomZ);
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, pos, _returnSpeed * 8f * Time.fixedDeltaTime);
        }
        else
        {
            _camera.localPosition = Vector3.Lerp(_camera.localPosition, _cameraOffset, _returnSpeed * Time.fixedDeltaTime);
        }
    }
}
