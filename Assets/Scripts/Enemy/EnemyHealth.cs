using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private int _experienceAfterDestroy = 10;
    [SerializeField] private Animator _animator;

    private PlayerExperience _playerExperience;

    public void Init(PlayerExperience playerExperience)
    {
        _playerExperience = playerExperience;
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyMe();
        }
        else
        {
            _animator.SetTrigger("Hit");
        }
    }

    private void DestroyMe()
    {
        _animator.SetBool("IsDead", true);
        _playerExperience.AddExp(_experienceAfterDestroy);

        EnemyAI _enemy = GetComponent<EnemyAI>();
        _enemy.StopAllCoroutines();
        _enemy.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        enabled = false;
    }

    public bool IsAlive() => _health > 0f;
}
