using System.Collections;
using UnityEngine;

public class SoundMaker : MonoBehaviour
{
    [SerializeField] private SphereCollider _collider;
    [SerializeField, Range(0.01f, 1f)] private float _audioDurationScale = 1f;
    [SerializeField] private bool _playOnAwake;
    private Coroutine _makeSoundRoutine;

    private void Awake()
    {
        _collider.enabled = false;
        if (_playOnAwake)
            MakeSound(_collider.radius, _audioDurationScale);
    }

    public void MakeSound(float radius, float duration)
    {
        if (_makeSoundRoutine != null) return;

        _collider.radius = radius;
        _collider.enabled = true;
        _makeSoundRoutine = StartCoroutine(MakeSoundIE(duration));
    }

    private IEnumerator MakeSoundIE(float duration)
    {
        yield return new WaitForSeconds(duration * _audioDurationScale);
        _collider.enabled = false;
        _makeSoundRoutine = null;
    }
}
