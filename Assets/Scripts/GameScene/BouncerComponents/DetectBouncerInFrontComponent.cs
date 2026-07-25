using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class DetectBouncerInFrontComponent : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<UniversalBouncer> OnBouncerDetected { get; private set; } = new();

    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private float _detectionDistance = 2f;
    [SerializeField] private LayerMask _detectionLayerMask;

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    public void DetectBouncerInFront()
    {
        Vector2 direction = _bouncer.Rigidbody.linearVelocity.normalized;
        Vector2 origin = (Vector2)transform.position + direction * 0.1f;

        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, _detectionDistance, _detectionLayerMask);
        foreach (var hit in hits)
        {
            if (hit.collider == null || hit.collider.gameObject == gameObject)
                continue;
            if (!hit.collider.TryGetComponent(out UniversalBouncer bouncer))
                continue;

            Debug.Log("Detected object in front: " + bouncer.name);
            OnBouncerDetected.Invoke(bouncer);
            return;
        }
    }
}
