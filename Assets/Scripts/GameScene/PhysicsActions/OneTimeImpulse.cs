using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(UniversalBouncer))]
public class OneTimeImpulse : MonoBehaviour
{
    [SerializeField] private UniversalBouncer _bouncer;

    [SerializeField] private float _force = 12;
    [SerializeField] private bool _useRandomAngle = true;
    [SerializeField, HideIf(nameof(_useRandomAngle))] private float _angle = 0;
    [SerializeField] private bool _applyOnStart = true;

    private void Start()
    {
        if (_applyOnStart)
            ApplyImpulse();
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    private Vector2 GetDirection()
    {
        var angle = _useRandomAngle ? Random.Range(0f, 360f) : _angle;
        return FloatTools.AngleToDirection(angle);
    }

    private void ApplyImpulse()
    {
        _bouncer.Rigidbody.AddForce(GetDirection() * _force, ForceMode2D.Impulse);
    }

}
