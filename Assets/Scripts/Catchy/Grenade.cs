using System.Collections;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float damage = 50f;
    [SerializeField] private float _delay;
    [SerializeField] private GameObject _explosionPrefab;

    private void Start()
    {
        StartCoroutine(ExplodeIE());
    }

    private IEnumerator ExplodeIE()
    {
        yield return new WaitForSeconds(_delay);
        Explode();
    }

    public void Explode()
    {
        GetComponent<Collider>().enabled = false;
        StopAllCoroutines();
        Destroy(gameObject);
        Explosion explosion = Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Explosion>();
        explosion.damage = damage;
    }
}
