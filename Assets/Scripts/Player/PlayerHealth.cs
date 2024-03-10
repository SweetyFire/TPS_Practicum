using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private RectTransform _valueStatusRect;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Animator _animator;

    private float _value;

    private void Awake()
    {
        _value = _maxValue;
        DrawHealthbar();
    }

    public bool IsAllive()
    {
        return _value > 0;
    }

    public void TakeDamage(float damage)
    {
        _value = Mathf.Clamp(_value - damage, 0, _maxValue);

        if (_value <= 0)
        {
            DestroyMe();
        }

        DrawHealthbar();
    }

    private void DestroyMe()
    {
        _animator.SetBool("IsDead", true);
        _gameplayUI.SetActive(false);
        _gameOverScreen.SetActive(true);
        GetComponent<PlayerController>().DisableInput();
        GetComponent<FireballCaster>().enabled = false;
        GetComponent<CameraRotation>().enabled = false;
    }

    private void DrawHealthbar()
    {
        _valueStatusRect.anchorMax = new(_value / _maxValue, 1f);
    }
}
