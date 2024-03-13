using UnityEngine;

public class FireballCaster : MonoBehaviour
{
    public float damage = 10f;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Fireball _fireballPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Fireball fireball = Instantiate(_fireballPrefab, _shootPoint.position, _shootPoint.rotation);
            fireball.damage = damage;
        }
    }
}
