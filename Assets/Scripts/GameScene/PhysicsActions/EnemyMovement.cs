using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(UniversalBouncer))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private UniversalBouncer _bouncer;

    [SerializeField] private float _maxMovementVelocity;
    [SerializeField] private float _movementForce = 0.5f;
    [SerializeField] private float _startForce = 0;

    private CancellationTokenSource _handleEnemyMovement;

    private Vector2 _movementDirection = Vector2.zero;

    private void Start()
    {
        _bouncer.OnBounced.AddListener(OnBounced);
        UniTaskTools.RenewToken(ref _handleEnemyMovement);

        _movementDirection = GetRandomDirection();
        ApplyImpulse();
        HandleMovement(_handleEnemyMovement.Token).Forget();
    }
    private void OnDestroy()
    {
        _bouncer.OnBounced.RemoveListener(OnBounced);
        UniTaskTools.KillToken(ref _handleEnemyMovement);
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }


    private Vector2 GetRandomDirection()
    {
        return FloatTools.AngleToDirection(Random.Range(0f, 360f));
    }
    private void ApplyImpulse()
    {
        _bouncer.Rigidbody.AddForce(_movementDirection * _startForce, ForceMode2D.Impulse);
    }
    private void OnBounced(UniversalBouncer bouncer, Collision2D collision)
    {
        _movementDirection = _bouncer.Rigidbody.linearVelocity.normalized;
    }

    private async UniTask HandleMovement(CancellationToken token)
    {
        while(token.IsCancellationRequested == false)
        {
            Debug.DrawRay((Vector2)gameObject.transform.position + ((Vector2)transform.right * 0.1f), _movementDirection, UnityEngine.Color.green);
            if(_bouncer.Rigidbody.linearVelocity.magnitude < _maxMovementVelocity)
                _bouncer.Rigidbody.AddForce(_movementDirection * _movementForce, ForceMode2D.Force);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
