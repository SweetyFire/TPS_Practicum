using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private int _experienceAfterDestroy = 10;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _speedThresholdForTakeDamage = 10f;
    [SerializeField] private AudioSource _hurtAudioSource;
    [SerializeField] private List<CustomizableSound> _hurtSounds = new();
    [SerializeField] private Light _light;

    public float Health => _health;

    private PlayerExperience _playerExperience;

    public void Init(PlayerExperience playerExperience)
    {
        _playerExperience = playerExperience;
    }

    private void OnCollisionEnter(Collision collision)
    {
        float objectSpeed = collision.rigidbody.velocity.magnitude;
        if (objectSpeed > _speedThresholdForTakeDamage)
        {
            TakeDamage(objectSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (damage > 0)
        {
            PlayHitSound();
        }

        if (_health <= 0)
        {
            DestroyMe();
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
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
        _playerExperience.AddExp(_experienceAfterDestroy);

        EnemyAI _enemy = GetComponent<EnemyAI>();
        _enemy.StopAllCoroutines();
        _enemy.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        enabled = false;
        _light.gameObject.SetActive(false);
    }

    public bool IsAlive() => _health > 0f;
}
