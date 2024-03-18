using UnityEngine;

public class GravityGun : MonoBehaviour
{
    [SerializeField] private float _force = 30f;
    [SerializeField] private Vector2 _viewportPoint;

    private Rigidbody _targetObject;
    private bool _isLocked;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_isLocked)
        {
            if (Input.GetMouseButtonDown(0))
            {
                TryCastToTarget();
            }
        }
        else
        {
            UpdateTargetPosition();
            if (Input.GetMouseButtonDown(0))
            {
                ReleaseTarget();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DropTarget();
            }
        }
    }

    private void UpdateTargetPosition()
    {
        _targetObject.position = transform.position;
        _targetObject.rotation = transform.rotation;
    }

    private void DropTarget()
    {
        _isLocked = false;
        _targetObject.isKinematic = false;
        _targetObject = null;
    }

    private void TryCastToTarget()
    {
        if (Physics.Raycast(_camera.ViewportPointToRay(_viewportPoint), out RaycastHit hit))
        {
            if (hit.rigidbody != null && hit.collider.TryGetComponent<Throwable>(out var _))
            {
                LockTarget(hit.rigidbody);
            }
        }
    }

    private void ReleaseTarget()
    {
        _targetObject.isKinematic = false;
        _targetObject.velocity = transform.forward * _force;
        _isLocked = false;
        _targetObject = null;
    }

    private void LockTarget(Rigidbody rigidbody)
    {
        _isLocked = true;
        _targetObject = rigidbody;
        _targetObject.isKinematic = true;
    }
}
