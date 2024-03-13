using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float damage = 25f;

    [SerializeField] private float _maxSize = 5f;
    [SerializeField] private float _sizeUpdateSpeed = 1f;
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
            playerHealth.TakeDamage(damage);
        }

        if (other.TryGetComponent<EnemyHealth>(out var enemyHealth))
        {
            enemyHealth.TakeDamage(damage);
        }

        if (other.TryGetComponent<Grenade>(out var grenade))
        {
            grenade.Explode();
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
