using Cysharp.Threading.Tasks;
using NUnit.Framework;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;

[RequireComponent (typeof(EdgeCollider2D))]
[RequireComponent (typeof(LineRenderer))]
public class TwinRope : DamageComponent
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private EdgeCollider2D _collider;

    [SerializeField] private EnemyMovement _twinA;
    [SerializeField] private EnemyMovement _twinB;

    private CancellationTokenSource _handleRope;
    private bool _isDisabled;

    private void Reset()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _collider = GetComponent<EdgeCollider2D>();
    }
    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleRope);
        _twinA.OnDisabled.AddListener(OnTwinADisabled);
        _twinB.OnDisabled.AddListener(OnTwinBDisabled);
        _twinA.OnDisableEnd.AddListener(OnEnabledAgain);


        HandleRope(_handleRope.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleRope);
        _twinA.OnDisabled.RemoveListener(OnTwinADisabled);
        _twinB.OnDisabled.RemoveListener(OnTwinBDisabled);
        _twinA.OnDisableEnd.RemoveListener(OnEnabledAgain);
    }

    private async UniTask HandleRope(CancellationToken token)
    {
        List<Vector2> points = new();
        while(token.IsCancellationRequested == false)
        {
            _lineRenderer.SetPosition(0, _twinA.transform.position);
            _lineRenderer.SetPosition(1, _twinB.transform.position);
            points.Clear();
            points.Add(transform.InverseTransformPoint(_twinA.transform.position));
            points.Add(transform.InverseTransformPoint(_twinB.transform.position));
            _collider.SetPoints(points);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isDisabled)
            return;
        if (!collision.gameObject.TryGetComponent(out HealthComponent otherHealth))
            return;
        var otherBouncer = otherHealth.GetComponent<UniversalBouncer>();
        if (!CanBeDamaged(_twinA.Bouncer, otherBouncer))
            return;

        otherHealth.TakeDamage(_damageAmount);
    }

    public void OnTwinADisabled(float time)
    {
        if (_isDisabled)
            return;
        _isDisabled = true;
        _twinB.DisableForSeconds(time);
        ////////// disable visual effect here ///////////
    }
    public void OnTwinBDisabled(float time)
    {
        if (_isDisabled)
            return;
        _isDisabled = true;
        _twinA.DisableForSeconds(time);
        ////////// disable visual effect here ///////////
    }
    public void OnEnabledAgain()
    {
        _isDisabled = false;
        ////////// Enable visual effect again ///////////
    }
}
