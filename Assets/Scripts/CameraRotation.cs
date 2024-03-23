using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform _cameraAxisTransform;
    [SerializeField] private float _rotationSpeed = 2f;

    [SerializeField] private float _minAngle;
    [SerializeField] private float _maxAngle;

    private void Update()
    {
        float rotY = transform.localEulerAngles.y + _rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X");
        transform.localEulerAngles = new(0, rotY, 0);

        float rotX = _cameraAxisTransform.localEulerAngles.x - _rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y");
        if (rotX > 180)
            rotX -= 360;

        rotX = Mathf.Clamp(rotX, _minAngle, _maxAngle);
        _cameraAxisTransform.localEulerAngles = new(rotX, 0, 0);
    }
}
