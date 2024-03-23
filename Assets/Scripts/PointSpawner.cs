using System.Collections;
using UnityEngine;

public class PointSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _parentSpawnPoint;
    [SerializeField] private float _minDelay = 1f;
    [SerializeField] private float _maxDelay = 2f;
    [SerializeField] private float _minPlayerDistanceToSpawn;
    [SerializeField] private float _maxPlayerDistanceToSpawn = 50f;
    [SerializeField] private PlayerController _playerController;
    private GameObject _currentObject;

    private void Awake()
    {
        StartCoroutine(SpawnIE());
    }

    private IEnumerator SpawnIE()
    {
        while (true)
        {
            if (_currentObject != null)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            yield return new WaitForSeconds(Random.Range(_minDelay, _maxDelay));
            CreateObject();
        }
    }

    private void CreateObject()
    {
        int pointIndex = Random.Range(0, _parentSpawnPoint.childCount);
        Vector3 createPos = _parentSpawnPoint.GetChild(pointIndex).position;

        float distToPlayer = Vector3.Distance(createPos, _playerController.transform.position);
        if (distToPlayer > _maxPlayerDistanceToSpawn) return;
        if (_minPlayerDistanceToSpawn > 0f)
        {
            if (distToPlayer < _minPlayerDistanceToSpawn) return;
        }

        Quaternion createRot = Quaternion.Euler(0f, Random.Range(-180f, 180f), 0f);
        _currentObject = Instantiate(_prefab, createPos, createRot, _parentSpawnPoint.GetChild(pointIndex));
    }
}
