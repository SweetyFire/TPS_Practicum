using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class CustomizableSound
{
    public AudioClip clip;
    [Range(-3f, 3f)] public float minPitch = 1f;
    [Range(-3f, 3f)] public float maxPitch = 1f;
    [Range(0f, 1f)] public float minVolume = 1f;
    [Range(0f, 1f)] public float maxVolume = 1f;
    public bool dynamicPan;
}

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _minDelayBetweenSounds = 1f;
    [SerializeField] private float _maxDelayBetweenSounds = 10f;
    [SerializeField] private List<CustomizableSound> _ambientSounds;
    private List<CustomizableSound> _clipsQueue = new();
    private Coroutine _playQueueRoutine;

    private void Awake()
    {
        _clipsQueue.AddRange(_ambientSounds);

        if (_audioSource.playOnAwake)
        {
            PlayQueue();
        }
    }

    public void PlayQueue()
    {
        _clipsQueue.Shuffle();

        if (_playQueueRoutine != null)
            StopCoroutine(_playQueueRoutine);

        _playQueueRoutine = StartCoroutine(PlayQueueIE());
    }

    private IEnumerator PlayQueueIE()
    {
        int soundIndex = 0;
        while (soundIndex < _clipsQueue.Count)
        {
            yield return new WaitForSeconds(Random.Range(_minDelayBetweenSounds, _maxDelayBetweenSounds));

            CustomizableSound sound = _clipsQueue[soundIndex];
            _audioSource.clip = sound.clip;
            _audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
            _audioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);

            if (sound.dynamicPan)
            {
                float curPan = Random.Range(0f, 1f) <= 0.5f ? -1f : 1f;
                _audioSource.panStereo = curPan;
                _audioSource.Play();

                float waitTime = 0.05f;
                float clipDuration = _audioSource.GetClipDuration();
                float clipRemainingTime = _audioSource.GetClipRemainingTime();
                while (_audioSource.isPlaying)
                {
                    waitTime = clipRemainingTime < waitTime ? clipRemainingTime : waitTime;
                    yield return new WaitForSeconds(waitTime);
                    _audioSource.panStereo = Mathf.Lerp(curPan, -curPan, _audioSource.time / clipDuration);
                    clipRemainingTime = _audioSource.GetClipRemainingTime();
                }
            }
            else
            {
                _audioSource.panStereo = 0f;
                _audioSource.Play();
                yield return new WaitForSeconds(_audioSource.GetClipDuration());
            }

            soundIndex++;
        }
        
        PlayQueue();
    }
}
