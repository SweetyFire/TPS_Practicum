using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _minDetectDistance = 10f;
    [SerializeField] private float _damage = 30f;
    [SerializeField] private List<Transform> _patrolPoints = new();

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
    }

    private void Update()
    {
        RotateToPlayerUpdate();
    }

    private void InitComponents()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void InitComponentsStart()
    {
        if (_player != null)
            _playerHealth = _player.GetComponent<PlayerHealth>();
    }

    private IEnumerator LogicIE()
    {
        float waitTime = 0.2f;
        while (true)
        {
            ChasePlayerUpdate();
            AttackUpdate(waitTime);
            PatrolUpdate();

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void PatrolUpdate()
    {
        if (_seePlayer)
            return;

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
        if (_player == null)
            return;

        _seePlayer = false;

        if (Vector3.Distance(transform.position, _player.position) > _minDetectDistance)
            return;

        Vector3 direction = _player.position - transform.position;

        if (Vector3.Angle(transform.forward, direction) > _viewAngle)
            return;

        if (!Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit))
            return;

        if (hit.collider.gameObject == _player.gameObject)
        {
            _seePlayer = true;
            _navMeshAgent.destination = _player.position;
        }
    }

    private void AttackUpdate(float waitTime)
    {
        if (_player == null)
            return;

        if (!_seePlayer)
            return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _playerHealth.TakeDamage(_damage * waitTime);
        }
    }

    private void RotateToPlayerUpdate()
    {
        if (!_seePlayer)
            return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            Vector3 direction = _player.position - transform.position;
            Quaternion rot = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * 6f);
        }
    }
}
