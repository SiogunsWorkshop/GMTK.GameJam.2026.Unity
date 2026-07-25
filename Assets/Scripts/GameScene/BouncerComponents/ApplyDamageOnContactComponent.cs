using System;
using UnityEngine;

[RequireComponent(typeof(UniversalBouncer))]
public class ApplyDamageOnContactComponent : MonoBehaviour
{
    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private int _damageAmount = 1;

    [SerializeField] private bool _allowFriendlyFire;

    private void OnEnable()
    {
        _bouncer.OnBounced.AddListener(HandleBounced);
    }

    private void OnDisable()
    {
        _bouncer.OnBounced.RemoveListener(HandleBounced);
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    private void HandleBounced(UniversalBouncer self, Collision2D other)
    {
        if (!other.gameObject.TryGetComponent(out HealthComponent otherHealth))
            return;

        // If it has a health component it MUST have a bouncer component, so we can safely get it
        var otherBouncer = otherHealth.GetComponent<UniversalBouncer>();
        if (!CanBeDamaged(self, otherBouncer))
            return;

        otherHealth.TakeDamage(_damageAmount);
    }

    private bool CanBeDamaged(UniversalBouncer self, UniversalBouncer other)
    {
        bool sameTeam = self.Team == other.Team;
        return !sameTeam || _allowFriendlyFire;
    }
}
