using UnityEngine;

public class FireballSource : MonoBehaviour
{
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private float _targetInSkyDistance;
    private Camera _camera;

    private void Awake()
    {
        InitComponents();
    }

    private void Update()
    {
        Ray ray = _camera.ViewportPointToRay(new(0.5f, 0.7f, 0f));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            _targetPoint.position = hit.point;
        }
        else
        {
            _targetPoint.position = ray.GetPoint(_targetInSkyDistance);
        }

        transform.LookAt(_targetPoint);
    }

    private void InitComponents()
    {
        _camera = Camera.main;
    }
}
