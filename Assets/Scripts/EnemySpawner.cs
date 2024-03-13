using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class EnemySpawnPoint
{
    public IReadOnlyList<EnemyAI> Enemies => _enemies;
    public int EnemyCount => _enemies.Count;
    public Transform SpawnPoint => _spawnPoint;

    private List<EnemyAI> _enemies = new();
    private float _lastTimeSpawned;
    private Transform _spawnPoint;

    public EnemySpawnPoint(Transform spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void CreateEnemy(EnemyAI createdEnemy)
    {
        _lastTimeSpawned = Time.time;
        _enemies.Add(createdEnemy);
    }

    public void RemoveDead()
    {
        for (int i = _enemies.Count - 1; i >= 0; i--)
        {
            if (_enemies[i].Health.IsAlive()) continue;

            _enemies.RemoveAt(i);
        }
    }

    public bool CanCreate(float delay) => Time.time - _lastTimeSpawned > delay;
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyPrefab;
    [SerializeField] private Transform _parentSpawnPoint;
    [SerializeField] private int _maxEnemiesOnPoint = 3;
    [SerializeField] private float _minSpawnDelay = 3f;
    [SerializeField] private float _maxSpawnDelay = 5f;

    [SerializeField] private PlayerController _player;
    [SerializeField] private Transform _parentPatrolPoints;

    private List<EnemySpawnPoint> _enemySpawnPoints = new();
    private PlayerExperience _playerExperience;

    private void Awake()
    {
        for (int i = 0; i < _parentSpawnPoint.childCount; i++)
        {
            _enemySpawnPoints.Add(new(_parentSpawnPoint.GetChild(i)));
        }

        _playerExperience = _player.GetComponent<PlayerExperience>();
    }

    private void Start()
    {
        StartCoroutine(SpawnIE());
    }

    private IEnumerator SpawnIE()
    {
        List<EnemySpawnPoint> canCreatePoints = new();

        while (true)
        {
            canCreatePoints.Clear();

            foreach (EnemySpawnPoint spawnPoint in _enemySpawnPoints)
            {
                spawnPoint.RemoveDead();

                if (spawnPoint.EnemyCount < _maxEnemiesOnPoint)
                {
                    canCreatePoints.Add(spawnPoint);
                }
            }

            if (canCreatePoints.Count > 0)
            {
                int pointIndex = Random.Range(0, canCreatePoints.Count);
                CreateEnemy(canCreatePoints[pointIndex]);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private void CreateEnemy(EnemySpawnPoint enemySpawnPoint)
    {
        if (!enemySpawnPoint.CanCreate(Random.Range(_minSpawnDelay, _maxSpawnDelay))) return;

        Vector3 createPos = enemySpawnPoint.SpawnPoint.position;
        Quaternion createRot = enemySpawnPoint.SpawnPoint.rotation;
        EnemyAI enemy = Instantiate(_enemyPrefab, createPos, createRot, enemySpawnPoint.SpawnPoint);

        List<Transform> patrolPoints = new();
        for (int i = 0; i < _parentPatrolPoints.childCount; i++)
        {
            patrolPoints.Add(_parentPatrolPoints.GetChild(i));
        }
        enemy.Init(_player.transform, patrolPoints);
        enemy.Health.Init(_playerExperience);

        enemySpawnPoint.CreateEnemy(enemy);
    }
}
