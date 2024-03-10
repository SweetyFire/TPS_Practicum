using UnityEngine;
using UnityEngine.Events;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private UnityEvent<PlayerController> _onCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            _onCollected?.Invoke(player);
            DestroyMe();
        }
    }

    private void DestroyMe()
    {
        Destroy(gameObject);
    }
}
