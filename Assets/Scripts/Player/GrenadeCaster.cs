using UnityEngine;

public class GrenadeCaster : MonoBehaviour
{
    [SerializeField] private Rigidbody _granadePrefab;
    [SerializeField] private Transform _shootTransform;
    [SerializeField] private float _force;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Rigidbody grenade = Instantiate(_granadePrefab.gameObject, _shootTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
            grenade.AddForce(_shootTransform.forward * _force);
        }
    }
}
