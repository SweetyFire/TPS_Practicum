using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _explosionPrefab;

    private void Start()
    {
        StartCoroutine(ExplodeIE());
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }

    private IEnumerator ExplodeIE()
    {
        yield return new WaitForSeconds(_delay);
        Explode();
    }

    private void Explode()
    {
        Destroy(gameObject);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
    }
}
