using UnityEngine;

public class GrenadeCaster : MonoBehaviour
{
    public float damage = 50f;

    [SerializeField] private Rigidbody _granadePrefab;
    [SerializeField] private Transform _shootTransform;
    [SerializeField] private float _force;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _shootCooldown = 10f;
    private float _cooldown;

    private void Update()
    {
        if (_cooldown > 0f)
        {
            _cooldown -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Rigidbody body = Instantiate(_granadePrefab.gameObject, _shootTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
            Grenade grenade = body.GetComponent<Grenade>();
            grenade.damage = damage;

            float plFallVelocity = Mathf.Clamp(_playerController.FallVelocity * 30f, -300f, 0f);
            float playerVelocity = _playerController.SpeedVector.y * _playerController.CurrentSpeed * 30f;
            body.AddForce(_shootTransform.forward * (_force + playerVelocity - plFallVelocity));

            _cooldown = _shootCooldown;
        }
    }
}
