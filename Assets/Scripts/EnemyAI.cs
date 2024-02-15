using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private List<Transform> _patrolPoints = new();
    [SerializeField] private Transform _player;

    private NavMeshAgent _navMeshAgent;
    private bool _isPlayerNoticed;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(LogicIE());
    }

    private IEnumerator LogicIE()
    {
        while (true)
        {
            PatrolUpdate();

            Vector3 direction = transform.position - _player.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position + Vector3.up, direction, out hit))
            {
                if (hit.collider.gameObject == _player.gameObject)
                {
                    _isPlayerNoticed = true;
                }
                else
                {
                    _isPlayerNoticed = false;
                }
            }
            else
            {
                _isPlayerNoticed = false;
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
        _navMeshAgent.destination = _patrolPoints[Random.Range(0, _patrolPoints.Count)].position;
    }
}
