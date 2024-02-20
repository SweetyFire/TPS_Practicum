using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float _flySpeed = 100f;
    [SerializeField] private float _lifeTime = 3f;

    private Rigidbody _rb;

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
        DestroyFireball();
    }

    private void InitComponents()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void MoveFixedUpdate()
    {
        _rb.velocity = _flySpeed * Time.fixedDeltaTime * transform.forward;
    }

    private void DestroyFireball()
    {
        Destroy(gameObject);
    }
}
