using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _viewAngle = 90f;
    [SerializeField] private float _minDetectDistance = 10f;
    [SerializeField] private List<Transform> _patrolPoints = new();

    private NavMeshAgent _navMeshAgent;

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
            if (CheckPlayer())
            {
                ChaseUpdate();
            }
            else
            {
                PatrolUpdate();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void PatrolUpdate()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            PickNewPatrolPoint();
        }
    }

    private void PickNewPatrolPoint()
    {
        _navMeshAgent.SetDestination(_patrolPoints[Random.Range(0, _patrolPoints.Count)].position);
    }

    private bool CheckPlayer()
    {
        if (_player != null)
        {
            if (Vector3.Distance(transform.position, _player.position) > _minDetectDistance)
                return false;

            Vector3 direction = _player.position - transform.position;

            if (Vector3.Angle(transform.forward, direction) > _viewAngle)
                return false;

            if (!Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit))
                return false;

            if (hit.collider.gameObject == _player.gameObject)
                return true;
        }

        return false;
    }

    private void ChaseUpdate()
    {
        _navMeshAgent.destination = _player.position;
    }
}
