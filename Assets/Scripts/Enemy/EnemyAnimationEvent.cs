using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;

    public void AttackEvent()
    {
        _enemyAI.Attack();
    }
}
