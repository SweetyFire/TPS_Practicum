using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    [SerializeField] private Animator _animator;

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
        EnemyAI _enemy = GetComponent<EnemyAI>();
        _enemy.StopAllCoroutines();
        _enemy.enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<NavMeshAgent>().enabled = false;
        enabled = false;
    }
}
