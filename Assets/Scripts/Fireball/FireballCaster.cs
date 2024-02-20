using UnityEngine;

public class FireballCaster : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private GameObject _fireballPrefab;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_fireballPrefab, _shootPoint.position, _shootPoint.rotation);
        }
    }
}
