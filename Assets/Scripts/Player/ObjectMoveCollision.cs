using UnityEngine;

public class ObjectMoveCollision : MonoBehaviour
{
    [SerializeField] private Transform _fromObject;
    [SerializeField] private Transform _toObject;
    [SerializeField] private float _castRadius = 0.1f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _approachSpeed = 10f;
    [SerializeField] private float _returnSpeed = 4f;
    [SerializeField] private float _offset;
    private Vector3 _defaultPosition;
    private float _maxDistance;

    private void Awake()
    {
        _defaultPosition = _fromObject.localPosition;
        _maxDistance = Vector3.Distance(transform.position, transform.position + _defaultPosition);
    }

    private void FixedUpdate()
    {
        Vector3 direction = GetDirection();

        if (Physics.SphereCast(transform.position, _castRadius, direction, out RaycastHit hit, _maxDistance, _layerMask))
        {
            Vector3 toPos = _toObject == null ? Vector3.zero : _toObject.localPosition;
            float distPercent = Mathf.Clamp01((hit.distance + _castRadius / 2f - _offset) / _maxDistance);
            Vector3 pos = Vector3.Lerp(toPos, _defaultPosition, distPercent);
            _fromObject.localPosition = Vector3.Lerp(_fromObject.localPosition, pos, _approachSpeed * Time.fixedDeltaTime);
        }
        else
        {
            _fromObject.localPosition = Vector3.Lerp(_fromObject.localPosition, _defaultPosition, _returnSpeed * Time.fixedDeltaTime);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = _toObject == null ? _defaultPosition : (_defaultPosition - _toObject.localPosition);
        return transform.rotation * dir.normalized;
    }
}
