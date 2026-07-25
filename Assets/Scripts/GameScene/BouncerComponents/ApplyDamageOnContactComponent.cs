using System;
using UnityEngine;

[RequireComponent(typeof(UniversalBouncer))]
public class ApplyDamageOnContactComponent : DamageComponent
{
    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private EnemyMovement _enemy;
    
    private bool _disabled = false;

    private void OnEnable()
    {
        _bouncer.OnBounced.AddListener(HandleBounced);
        if( _enemy != null)
        {
            _enemy.OnDisabled.AddListener(TurnOff);
            _enemy.OnDisableEnd.AddListener(TurnOn);
        }
    }

    private void OnDisable()
    {
        _bouncer.OnBounced.RemoveListener(HandleBounced);
        if (_enemy != null)
        {
            _enemy.OnDisabled.RemoveListener(TurnOff);
            _enemy.OnDisableEnd.RemoveListener(TurnOn);
        }
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    private void HandleBounced(UniversalBouncer self, Collision2D other)
    {
        if (_disabled)
            return;
        if (!other.gameObject.TryGetComponent(out HealthComponent otherHealth))
            return;

        // If it has a health component it MUST have a bouncer component, so we can safely get it
        var otherBouncer = otherHealth.GetComponent<UniversalBouncer>();
        if (!CanBeDamaged(self, otherBouncer))
            return;

        otherHealth.TakeDamage(_damageAmount);
    }
    private void TurnOff(float notUsed)
    {
        _disabled = true;
    }
    private void TurnOn()
    {
        _disabled = false;
    }
}
