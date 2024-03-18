using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _minDetectDistance = 10f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private List<Transform> _patrolPoints = new();
    [SerializeField] private Animator _animator;
    [SerializeField] private float _attackDistance = 1f;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _footstepAudioSource;
    [SerializeField] private List<CustomizableSound> _voiceSounds = new();
    [SerializeField] private SoundMaker _soundMaker;

    public EnemyHealth Health { get; private set; }

    private NavMeshAgent _navMeshAgent;
    private PlayerHealth _playerHealth;
    private bool _seePlayer;

    private void Awake()
    {
        InitComponents();
    }

    private void Start()
    {
        InitComponentsStart();
        StartCoroutine(LogicIE());
        StartCoroutine(MakeSoundIE());
    }

    private void Update()
    {
        RotateToPlayerUpdate();
    }

    private void OnTriggerEnter(Collider other)
    {
        TryHearSound(other);
    }

    private void OnTriggerExit(Collider other)
    {
        TryHearSound(other);
    }

    private void TryHearSound(Collider other)
    {
        if (_seePlayer || !_navMeshAgent.enabled) return;

        if (other.TryGetComponent<SoundMaker>(out var _))
        {
            _navMeshAgent.destination = other.transform.position;
        }
    }

    public void Init(Transform player, IEnumerable<Transform> patrolPoints)
    {
        _player = player;
        _patrolPoints = patrolPoints.ToList();
    }

    private void InitComponents()
    {
        Health = GetComponent<EnemyHealth>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void InitComponentsStart()
    {
        if (_player != null) _playerHealth = _player.GetComponent<PlayerHealth>();
    }

    private IEnumerator LogicIE()
    {
        float waitTime = 0.2f;
        while (true)
        {
            ChasePlayerUpdate();
            AttackUpdate();
            PatrolUpdate();

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void PatrolUpdate()
    {
        if (_seePlayer) return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            PickNewPatrolPoint();
        }
    }

    private void PickNewPatrolPoint()
    {
        _navMeshAgent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Count)].position);
    }

    private void ChasePlayerUpdate()
    {
        if (_player == null) return;

        _seePlayer = false;
        if (!_playerHealth.IsAlive()) return;

        if (Vector3.Distance(transform.position, _player.position) > _minDetectDistance) return;

        Vector3 direction = _player.position - transform.position;

        if (Vector3.Angle(transform.forward, direction) > _viewAngle) return;

        if (!Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit)) return;

        if (hit.collider.gameObject == _player.gameObject)
        {
            _seePlayer = true;
            _navMeshAgent.destination = _player.position;
        }
    }

    private void AttackUpdate()
    {
        if (_player == null) return;
        if (!_seePlayer) return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _animator.SetTrigger("Attack");
        }
    }

    private void RotateToPlayerUpdate()
    {
        if (!_seePlayer) return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Vector3 direction = _player.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 6f);
        }
    }

    private IEnumerator MakeSoundIE()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(10f, 30f));
            if (_seePlayer) continue;
            int soundIndex = Random.Range(0, _voiceSounds.Count);
            PlayOneShotSound(_voiceSounds[soundIndex]);
        }
    }

    public void MakeSound(float radius, CustomizableSound sound)
    {
        _soundMaker.MakeSound(radius, _audioSource.GetClipDuration(sound.clip));
    }

    public void PlayOneShotSound(CustomizableSound sound)
    {
        _audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        _audioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);
        _audioSource.PlayOneShot(sound.clip);
    }

    public void PlayFootsteps(CustomizableSound sound)
    {
        _footstepAudioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        _footstepAudioSource.volume = Random.Range(sound.minVolume, sound.maxVolume);
        _footstepAudioSource.PlayOneShot(sound.clip);
    }

    public void Attack()
    {
        if (!_seePlayer) return;
        if (!_navMeshAgent.enabled) return;
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance + _attackDistance) return;

        _playerHealth.TakeDamage(_damage);
    }
}
