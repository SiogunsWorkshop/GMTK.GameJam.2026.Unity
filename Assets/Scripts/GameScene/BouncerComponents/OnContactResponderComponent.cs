using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniversalBouncer))]
public class OnContactResponderComponent : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<UniversalBouncer, UniversalBouncer> OnBounce { get; private set; } = new();

    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private Team _teamWhitelist = Team.None;

    private void OnEnable()
    {
        _bouncer.OnBounced.AddListener(HandleBounce);
    }

    private void OnDisable()
    {
        _bouncer.OnBounced.RemoveListener(HandleBounce);
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    private void HandleBounce(UniversalBouncer arg0, Collision2D arg1)
    {
        if (!arg1.collider.TryGetComponent(out UniversalBouncer otherBouncer))
            return;

        if (_teamWhitelist != Team.None && otherBouncer.Team != _teamWhitelist)
            return;

        OnBounce.Invoke(arg0, otherBouncer);
    }
}
