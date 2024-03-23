using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _flySpeed = 100f;
    [SerializeField] private float _lifeTime = 3f;
    [SerializeField] private ParticleSystem _particles;
    public float damage = 10f;

    private Rigidbody _rb;
    private bool _isDestroyed;

    private void Awake()
    {
        InitComponents();
        Invoke(nameof(DestroyFireball), _lifeTime);
    }

    private void FixedUpdate()
    {
        MoveFixedUpdate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        DamageEnemy(collision);
        DestroyFireball();
    }

    private void InitComponents()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void MoveFixedUpdate()
    {
        if (_isDestroyed) return;

        _rb.velocity = _flySpeed * Time.fixedDeltaTime * transform.forward;
    }

    private void DestroyFireball()
    {
        if (_isDestroyed) return;

        _isDestroyed = true;
        _rb.velocity = Vector3.zero;
        _rb.isKinematic = true;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        StartCoroutine(DestroyIE());
    }

    private IEnumerator DestroyIE()
    {
        yield return new WaitForSeconds(_particles.main.startLifetime.constantMax);
        Destroy(gameObject);
    }

    private void DamageEnemy(Collision collision)
    {
        if (collision.transform.TryGetComponent<EnemyHealth>(out var enemy))
        {
            enemy.TakeDamage(damage);
        }
    }
}
