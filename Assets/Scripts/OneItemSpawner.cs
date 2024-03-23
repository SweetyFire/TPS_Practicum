using UnityEngine;

public class OneItemSpawner : MonoBehaviour
{
    [SerializeField] private Transform _parentPoint;
    [SerializeField] private GameObject _prefabToSpawn;

    private void Awake()
    {
        Spawn();
    }

    private void Spawn()
    {
        int pointIndex = Random.Range(0, _parentPoint.childCount);
        Transform point = _parentPoint.GetChild(pointIndex);

        Vector3 position = point.position;
        Quaternion rotation = Quaternion.Euler(0f, Random.Range(0, 360f), 0f);
        Instantiate(_prefabToSpawn, position, rotation, point);
    }
}
