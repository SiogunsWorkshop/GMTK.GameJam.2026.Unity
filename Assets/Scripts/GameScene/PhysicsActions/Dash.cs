using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class Dash : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnDash { get; private set; } = new();

    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private float _force = 12;

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    public void DashAlongCurrentVelocity()
    {
        DashNow(_bouncer.Rigidbody.linearVelocity);
    }

    public void DashNow(Vector2 direction)
    {
        _bouncer.Rigidbody.AddForce(direction.normalized * _force, ForceMode2D.Impulse);
        OnDash.Invoke();
    }
}
