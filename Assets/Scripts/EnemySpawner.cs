using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyPrefab;
    [SerializeField] private int _maxEnemies = 3;
    [SerializeField] private float _minSpawnDelay = 3f;
    [SerializeField] private float _maxSpawnDelay = 5f;

    [Space, SerializeField] private float _minPlayerDistanceToSpawn;
    [SerializeField] private float _maxPlayerDistanceToSpawn = 50f;
    [SerializeField] private bool _spawnOutOfSight = true;
    [SerializeField] private LayerMask _castLayerMask;

    [Space, SerializeField] private PlayerController _player;
    [SerializeField] private Transform _parentPatrolPoints;

    private List<EnemyAI> _spawnedEnemies = new();
    private List<Transform> _patrolPoints = new();
    private PlayerExperience _playerExperience;

    private void Awake()
    {
        _playerExperience = _player.GetComponent<PlayerExperience>();

        for (int i = 0; i < _parentPatrolPoints.childCount; i++)
        {
            _patrolPoints.Add(_parentPatrolPoints.GetChild(i));
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnIE());
    }

    private IEnumerator SpawnIE()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(_minSpawnDelay, _maxSpawnDelay));

            for (int i = _spawnedEnemies.Count - 1; i >= 0; i--)
            {
                if (_spawnedEnemies[i].Health.IsAlive()) continue;
                _spawnedEnemies.RemoveAt(i);
            }

            if (_spawnedEnemies.Count < _maxEnemies)
            {
                CreateEnemy();
            }
        }
    }

    private void CreateEnemy()
    {
        Vector3 playerPos = _player.transform.position + Vector3.up;
        float distanceToPlayer = Vector3.Distance(transform.position, playerPos);

        if (distanceToPlayer > _maxPlayerDistanceToSpawn) return;

        if (_minPlayerDistanceToSpawn > 0)
        {
            if (distanceToPlayer <= _minPlayerDistanceToSpawn) return;
        }

        if (_spawnOutOfSight)
        {
            if (Physics.Linecast(transform.position, playerPos, out RaycastHit hit, _castLayerMask))
            {
                if (hit.collider.gameObject == _player.gameObject)
                {
                    return;
                }
            }

            if (Physics.Linecast(transform.position, _player.CameraCollider.transform.position, out RaycastHit camHit, _castLayerMask))
            {
                if (camHit.collider == _player.CameraCollider)
                {
                    return;
                }
            }
        }

        Vector3 createPos = transform.position;
        Quaternion createRot = transform.rotation;
        EnemyAI enemy = Instantiate(_enemyPrefab, createPos, createRot, transform);
        _spawnedEnemies.Add(enemy);
        enemy.Init(_player.transform, _patrolPoints);
        enemy.Health.Init(_playerExperience);
    }
}
