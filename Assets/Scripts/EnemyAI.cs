using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints = new();
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewAngle = 90f;

    private NavMeshAgent _navMeshAgent;
    private bool _isPlayerNoticed;

    private void Awake()
    {
        InitComponents();
        StartCoroutine(LogicIE());
    }

    private void InitComponents()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private IEnumerator LogicIE()
    {
        while (true)
        {
            NoticePlayerUpdate();
            ChaseUpdate();
            PatrolUpdate();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void PatrolUpdate()
    {
        if (_isPlayerNoticed) return;

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            PickNewPatrolPoint();
        }
    }

    private void PickNewPatrolPoint()
    {
        _navMeshAgent.destination = _patrolPoints[Random.Range(0, _patrolPoints.Count)].position;
    }

    private void NoticePlayerUpdate()
    {
        _isPlayerNoticed = false;

        if (_player != null)
        {
            Vector3 direction = _player.position - transform.position;
            if (Vector3.Angle(transform.forward, direction) < _viewAngle)
            {
                if (Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit))
                {
                    if (hit.collider.gameObject == _player.gameObject)
                    {
                        _isPlayerNoticed = true;
                    }
                }
            }
        }
    }

    private void ChaseUpdate()
    {
        if (_isPlayerNoticed)
        {
            _navMeshAgent.destination = _player.position;
        }
    }
}
