using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimGameOverScreen : MonoBehaviour
{
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private TextMeshProUGUI _gameOverText;
    [SerializeField] private TextMeshProUGUI _hintText;

    [SerializeField] private Color _disabledBackgroundColor;
    [SerializeField] private Color _disabledGameOverTextColor;
    [SerializeField] private Color _disabledHintTextColor;
    [SerializeField] private float _animationSpeed = 3f;

    [SerializeField] private Color _defaultBackgroundColor;
    [SerializeField] private Color _defaultGameOverTextColor;
    [SerializeField] private Color _defaultHintTextColor;
    private bool _isAnimated;

    public void Init()
    {
        DisableAnim();
    }

    private void Update()
    {
        if (_isAnimated)
        {
            _backgroundImage.color = Color.Lerp(_backgroundImage.color, _defaultBackgroundColor, _animationSpeed * Time.deltaTime);
            _gameOverText.color = Color.Lerp(_gameOverText.color, _defaultGameOverTextColor, _animationSpeed * Time.deltaTime);
            _hintText.color = Color.Lerp(_hintText.color, _defaultHintTextColor, _animationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        _isAnimated = true;
    }

    private void OnDisable()
    {
        DisableAnim();
    }

    private void DisableAnim()
    {
        _backgroundImage.color = _disabledBackgroundColor;
        _gameOverText.color = _disabledGameOverTextColor;
        _hintText.color = _disabledHintTextColor;
        _isAnimated = false;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _backgroundImage.color = _defaultBackgroundColor;
        _gameOverText.color = _defaultGameOverTextColor;
        _hintText.color = _defaultHintTextColor;
    }
#endif
}
