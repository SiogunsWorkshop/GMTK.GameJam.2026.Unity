using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class InterruptEnemyInFrontComponent : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<UniversalBouncer> OnBouncerDetected { get; private set; } = new();

    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private float _detectionDistance = 2f;
    [SerializeField] private LayerMask _detectionLayerMask;
    [SerializeField] private float _disableDuration = 5f;

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    public void DetectBouncerInFront()
    {
        Vector2 direction = _bouncer.Rigidbody.linearVelocity.normalized;
        if(_bouncer.Rigidbody.linearVelocity.magnitude <= 0.005f)
            direction = Vector2.up;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, _detectionDistance, _detectionLayerMask);
        Debug.DrawRay(origin, direction, Color.magenta, 2f);
        foreach (var hit in hits)
        {
            if (hit.collider == null || hit.collider.gameObject == gameObject)
                continue;
            if (!hit.collider.TryGetComponent(out UniversalBouncer bouncer))
                continue;

            OnBouncerDetected.Invoke(bouncer);
            TryInterruptBouncer(bouncer);
            return;
        }
    }

    private void TryInterruptBouncer(UniversalBouncer bouncer)
    {
        if (!bouncer.TryGetComponent(out EnemyMovement enemyMovement))
            return;

        enemyMovement.DisableForSeconds(_disableDuration);
    }
}
