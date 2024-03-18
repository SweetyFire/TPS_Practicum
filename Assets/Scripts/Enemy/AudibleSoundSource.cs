using UnityEngine;

public enum FinalAction
{
    None,
    Destroy,
    Disable
}

public class AudibleSoundSource : MonoBehaviour
{
    public float Radius => _audioSource.maxDistance;
    public float Duration => _audioSource.GetClipDuration();
    public AudioClip Clip => _audioSource.clip;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private FinalAction _finalAction;

    private void Update()
    {
        CheckFinalActionUpdate();
    }

    private void CheckFinalActionUpdate()
    {
        if (_finalAction == FinalAction.None) return;

        if (!_audioSource.IsPlaying())
        {
            switch (_finalAction)
            {
                case FinalAction.Destroy:
                    Destroy(gameObject);
                    break;

                case FinalAction.Disable:
                    gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void Play(CustomizableSound sound)
    {
        SetRandomPitch(sound);
        SetRandomVolume(sound);
        _audioSource.clip = sound.clip;
        _audioSource.Play();
    }  

    private void SetRandomPitch(CustomizableSound sound)
    {
        if ((sound.minPitch > 1f || sound.minPitch < 1f) && (sound.maxPitch >= sound.minPitch))
        {
            _audioSource.pitch = Mathf.Clamp(Random.Range(sound.minPitch, sound.maxPitch), -3f, 3f);
        }
    }

    private void SetRandomVolume(CustomizableSound sound)
    {
        if (sound.minVolume < 1f && sound.maxVolume >= sound.minVolume)
        {
            _audioSource.volume = Mathf.Clamp(Random.Range(sound.minVolume, sound.maxVolume), 0f, 1f);
        }
    }
}
