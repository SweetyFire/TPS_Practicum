using UnityEngine;

public class FireballCaster : MonoBehaviour
{
    public float damage = 10f;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Fireball _fireballPrefab;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private CustomizableSound _castSound;
    [SerializeField] private SoundMaker _soundMaker;
    [SerializeField] private float _hearSoundDistance = 6f;
    [SerializeField] private float _shootCooldown = 0.2f;

    private float _cooldown;

    private void Update()
    {
        if (_cooldown > 0f)
        {
            _cooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            Fireball fireball = Instantiate(_fireballPrefab, _shootPoint.position, _shootPoint.rotation);
            fireball.damage = damage;
            PlayCastSound();
            _cooldown = _shootCooldown;
        }
    }

    private void PlayCastSound()
    {
        _audioSource.pitch = Random.Range(_castSound.minPitch, _castSound.maxPitch);
        _audioSource.volume = Random.Range(_castSound.minVolume, _castSound.maxVolume);
        _audioSource.PlayOneShot(_castSound.clip);
        _soundMaker.MakeSound(_hearSoundDistance, _audioSource.GetClipDuration(_castSound.clip));
    }
}
