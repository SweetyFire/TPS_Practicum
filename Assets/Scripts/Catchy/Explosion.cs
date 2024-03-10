using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float _maxSize = 5f;
    [SerializeField] private float _sizeUpdateSpeed = 1f;
    [SerializeField] private float _damage = 25f;
    [SerializeField] private Transform _particles;

    private void Awake()
    {
        _particles.SetParent(null);
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (transform.localScale.x >= _maxSize)
        {
            DestroyMe();
            return;
        }

        transform.localScale += _sizeUpdateSpeed * Time.deltaTime * Vector3.one;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out var playerHealth))
        {
            playerHealth.TakeDamage(_damage);
        }

        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(_damage);
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
