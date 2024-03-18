using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private List<CustomizableSound> _footstepSounds = new();
    [SerializeField] private CustomizableSound _deathSound;
    [SerializeField] private CustomizableSound _landedSound;
    private bool _canFootstep = true;

    public void Footstep()
    {
        if (!_canFootstep) return;
        int soundIndex = Random.Range(0, _footstepSounds.Count);
        _playerController.PlayFootsteps(_footstepSounds[soundIndex]);
        _canFootstep = false;
        StartCoroutine(CanFootstepIE());
    }

    public void Death()
    {
        _playerController.PlayOneShotSound(_deathSound);
    }

    public void Landed()
    {
        _playerController.PlayFootsteps(_landedSound, 9f);
    }

    private IEnumerator CanFootstepIE()
    {
        yield return new WaitForSeconds(0.1f);
        _canFootstep = true;
    }
}
