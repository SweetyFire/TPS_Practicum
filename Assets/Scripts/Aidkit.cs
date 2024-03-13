using UnityEngine;

public class Aidkit : MonoBehaviour
{
    [SerializeField] private float _health = 10f;
    [SerializeField] private Transform _particles;

    public void Collect(PlayerController playerController)
    {
        CreateParticles();
        playerController.GetComponent<PlayerHealth>().TakeDamage(-_health);
    }

    private void CreateParticles()
    {
        _particles.SetParent(transform.parent);
        _particles.gameObject.SetActive(true);
    }
}
