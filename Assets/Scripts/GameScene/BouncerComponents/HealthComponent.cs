using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class HealthComponent : MonoBehaviour
{
    [field: SerializeField, FoldoutGroup("Events")] public UnityEvent<int> OnHealthChanged { get; private set; } = new();
    [field: SerializeField, FoldoutGroup("Events")] public UnityEvent OnDamaged { get; private set; } = new();
    [field: SerializeField, FoldoutGroup("Events")] public UnityEvent OnHealed { get; private set; } = new();
    [field: SerializeField, FoldoutGroup("Events")] public UnityEvent OnDeath { get; private set; } = new();

    [field: SerializeField] public int MaxHealth { get; private set; } = 3;
    [field: ShowInInspector, ReadOnly] public int CurrentHealth { get; private set; }
    [field: SerializeField] public float PostDamageInvincibilityPeriod { get; private set; } = 1f;

    public bool IsInvincible => Time.time - _lastDamageTime < PostDamageInvincibilityPeriod;
    public bool IsDead => CurrentHealth <= 0;

    private bool _hasDied;
    private float _lastDamageTime;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (_hasDied) return;
        if (damage <= 0) return;

        SetHealth(CurrentHealth - damage);
        _lastDamageTime = Time.time;
        OnDamaged.Invoke();

        TryDie();
    }

    public void Heal(int healAmount)
    {
        if (_hasDied) return;
        if (healAmount <= 0) return;

        SetHealth(CurrentHealth + healAmount);
        OnHealed.Invoke();
    }

    private void SetHealth(int health)
    {
        CurrentHealth = Mathf.Clamp(health, 0, MaxHealth);
        OnHealthChanged.Invoke(CurrentHealth);
    }

    private void TryDie()
    {
        if (_hasDied || !IsDead) return;

        _hasDied = true;
        CurrentHealth = 0;

        OnDeath.Invoke();
    }
}
