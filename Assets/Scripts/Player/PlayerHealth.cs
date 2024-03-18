using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxValue => _maxValue;

    [SerializeField] private float _maxValue = 100f;
    [SerializeField] private RectTransform _valueStatusRect;
    [SerializeField] private GameObject _gameplayUI;
    [SerializeField] private GameObject _gameOverScreen;
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _hurtAudioSource;
    [SerializeField] private List<CustomizableSound> _hurtSounds = new();

    private float _value;

    private void Awake()
    {
        _value = _maxValue;
        DrawHealthbar();
    }

    public bool IsAlive() => _value > 0f;

    public void TakeDamage(float damage)
    {
        _value = Mathf.Clamp(_value - damage, 0, _maxValue);

        if (damage > 0)
        {
            PlayHitSound();
        }

        if (_value <= 0)
        {
            DestroyMe();
        }

        DrawHealthbar();
    }

    public void SetMaxValue(float maxValue)
    {
        _maxValue = maxValue;
    }

    private void PlayHitSound()
    {
        int soundIndex = Random.Range(0, _hurtSounds.Count);
        PlayOneShotSound(_hurtSounds[soundIndex]);
    }

    private void PlayOneShotSound(CustomizableSound sound)
    {
        _hurtAudioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        _hurtAudioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);
        _hurtAudioSource.PlayOneShot(sound.clip);
    }

    private void DestroyMe()
    {
        _animator.SetBool("IsDead", true);
        _gameplayUI.SetActive(false);
        _gameOverScreen.SetActive(true);
        GetComponent<PlayerController>().DisableInput();
        GetComponent<FireballCaster>().enabled = false;
        GetComponent<CameraRotation>().enabled = false;
        GetComponent<GrenadeCaster>().enabled = false;
    }

    private void DrawHealthbar()
    {
        _valueStatusRect.anchorMax = new(_value / _maxValue, 1f);
    }
}
