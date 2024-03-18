using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : MonoBehaviour
{
    [SerializeField] private EnemyAI _enemyAI;
    [SerializeField] private List<CustomizableSound> _footstepSounds = new();
    [SerializeField] private CustomizableSound _deathSound;

    public void AttackEvent()
    {
        _enemyAI.Attack();
    }

    public void Footstep()
    {
        int soundIndex = Random.Range(0, _footstepSounds.Count);
        _enemyAI.PlayFootsteps(_footstepSounds[soundIndex]);
    }

    public void Death()
    {
        _enemyAI.PlayOneShotSound(_deathSound);
        _enemyAI.MakeSound(10f, _deathSound);
    }
}
