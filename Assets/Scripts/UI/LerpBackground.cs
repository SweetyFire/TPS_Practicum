using UnityEngine;
using UnityEngine.UI;

public class LerpBackground : MonoBehaviour
{
    [SerializeField] private Color _enabledColor;
    [SerializeField] private Color _disabledColor;
    [SerializeField] private float _updateColorSpeed = 4f;

    private Color _currentColor;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _currentColor = _enabledColor;
    }

    private void Update()
    {
        _image.color = Color.Lerp(_image.color, _currentColor, Time.deltaTime * _updateColorSpeed);
    }

    private void OnEnable()
    {
        _currentColor = _enabledColor;
    }

    private void OnDisable()
    {
        _currentColor = _disabledColor;
        _image.color = _disabledColor;
    }
#if UNITY_EDITOR
    private void OnValidate()
    {
        Image img = GetComponent<Image>();
        if (gameObject.activeInHierarchy)
        {
            img.color = _enabledColor;
            _currentColor = _enabledColor;
        }
        else
        {
            img.color = _disabledColor;
            _currentColor = _disabledColor;
        }
    }
#endif
}
