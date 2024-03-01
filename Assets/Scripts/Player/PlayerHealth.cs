using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private RectTransform _valueStatusRect;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameOverScreen;

    private float _value;

    private void Awake()
    {
        _value = _maxValue;
        DrawHealthbar();
    }

    public void TakeDamage(float damage)
    {
        _value -= damage;
        if (_value <= 0)
        {
            DestroyMe();
        }

        DrawHealthbar();
    }

    private void DestroyMe()
    {
        _gameplayUI.SetActive(false);
        _gameOverScreen.SetActive(true);
        GetComponent<PlayerController>().enabled = false;
        GetComponent<FireballCaster>().enabled = false;
        GetComponent<CameraRotation>().enabled = false;
    }

    private void DrawHealthbar()
    {
        _valueStatusRect.anchorMax = new(_value / _maxValue, 1f);
    }
}
