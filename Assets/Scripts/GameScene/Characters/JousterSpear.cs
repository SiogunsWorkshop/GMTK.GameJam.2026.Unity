using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

[RequireComponent (typeof(CapsuleCollider2D))]
public class JousterSpear : DamageComponent
{
    [SerializeField] private EnemyMovement _jouster;

    private CapsuleCollider2D _collider;

    private CancellationTokenSource _handleSpear;

    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleSpear);
        HandleSpear(_handleSpear.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleSpear);
    }
    private void Reset()
    {
        _collider = GetComponent<CapsuleCollider2D>();
        _collider.isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out HealthComponent otherHealth))
            return;
        var otherBouncer = otherHealth.GetComponent<UniversalBouncer>();
        if (!CanBeDamaged(_jouster.Bouncer, otherBouncer))
            return;

        otherHealth.TakeDamage(_damageAmount);
    }

    private async UniTask HandleSpear(CancellationToken token)
    {
        while(token.IsCancellationRequested == false)
        {
            gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, _jouster.MovementDirection);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
