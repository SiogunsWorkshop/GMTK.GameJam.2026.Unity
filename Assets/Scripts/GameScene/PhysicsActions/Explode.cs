using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class Explode : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnExplode { get; private set; } = new();
    [field: SerializeField] public UnityEvent<UniversalBouncer> OnAffectedByExplosion { get; private set; } = new();

    [SerializeField] private float _force = 12;
    [SerializeField] private float _radius = 5;
    [SerializeField] private AnimationCurve _forceFalloff = AnimationCurve.Linear(0, 1, 1, 0);

    public void ExplodeNow()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (var collider in colliders)
        {
            if (collider.gameObject == gameObject)
                continue;

            if (!collider.TryGetComponent(out UniversalBouncer bouncer))
                continue;

            var direction = (bouncer.transform.position - transform.position).normalized;
            var distance = Vector2.Distance(bouncer.transform.position, transform.position);
            var forceMultiplier = _forceFalloff.Evaluate(distance / _radius);
            bouncer.Rigidbody.AddForce(_force * forceMultiplier * direction, ForceMode2D.Impulse);

            OnAffectedByExplosion.Invoke(bouncer);
        }
    }
}
