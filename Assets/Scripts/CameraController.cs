using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Transform _firstPersonTransform;
    [SerializeField] private ObjectMoveCollision _cameraCollision;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _firstPersonShootPoint;

    private bool _firstPerson;
    private Vector3 _defaultShootPointLocalPos;

    private void Awake()
    {
        _defaultShootPointLocalPos = _shootPoint.localPosition;
    }

    public void ToggleFirstPerson(bool enable)
    {
        _firstPerson = enable;
        _cameraCollision.enabled = !enable;

        if (_firstPerson)
        {
            transform.localPosition += _firstPersonTransform.localPosition;
            _cameraTransform.localPosition = Vector3.zero;
            _shootPoint.localPosition = _firstPersonShootPoint.localPosition;
        }
        else
        {
            transform.localPosition -= _firstPersonTransform.localPosition;
            _shootPoint.localPosition = _defaultShootPointLocalPos;
        }
    }
}
