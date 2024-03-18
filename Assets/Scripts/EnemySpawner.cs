using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyPrefab;
    [SerializeField] private int _maxEnemies = 3;
    [SerializeField] private float _minSpawnDelay = 3f;
    [SerializeField] private float _maxSpawnDelay = 5f;

    [Space, SerializeField] private PlayerController _player;
    [SerializeField] private Transform _parentPatrolPoints;

    private List<EnemyAI> _spawnedEnemies = new();
    private PlayerExperience _playerExperience;

    private void Awake()
    {
        _playerExperience = _player.GetComponent<PlayerExperience>();
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
        Vector3 createPos = transform.position;
        Quaternion createRot = transform.rotation;
        EnemyAI enemy = Instantiate(_enemyPrefab, createPos, createRot, transform);
        _spawnedEnemies.Add(enemy);

        List<Transform> patrolPoints = new();
        for (int i = 0; i < _parentPatrolPoints.childCount; i++)
        {
            patrolPoints.Add(_parentPatrolPoints.GetChild(i));
        }
        enemy.Init(_player.transform, patrolPoints);
        enemy.Health.Init(_playerExperience);
    }
}
