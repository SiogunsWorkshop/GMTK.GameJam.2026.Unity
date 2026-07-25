using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(UniversalBouncer))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private UniversalBouncer _bouncer;

    [SerializeField] private float _force = 12;

    private CancellationTokenSource _handleEnemyMovement;

    private Vector2 _movementDirection = Vector2.zero;

    private void Start()
    {
        _bouncer.OnBounced.AddListener(OnBounced);
        GetDirection();
        ApplyImpulse();
        UniTaskTools.RenewToken(ref _handleEnemyMovement);
        HandleMovement(_handleEnemyMovement.Token).Forget();
    }
    private void OnDestroy()
    {
        _bouncer.OnBounced.RemoveListener(OnBounced);
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }


    private Vector2 GetDirection()
    {
        return FloatTools.AngleToDirection(Random.Range(0f, 360f));
    }
    private void ApplyImpulse()
    {
        _bouncer.Rigidbody.AddForce(_movementDirection * _force, ForceMode2D.Force);
    }
    private void OnBounced(UniversalBouncer bouncer, Collision2D collision)
    {
        Vector2 incoming = -collision.relativeVelocity;
        Vector2 normal = collision.contacts[0].normal;
        _movementDirection = Vector2.Reflect(incoming, normal).normalized;
    }

    private async UniTask HandleMovement(CancellationToken token)
    {
        while(token.IsCancellationRequested == false)
        {
            Debug.DrawRay((Vector2)gameObject.transform.position + (0.1f * _movementDirection), _movementDirection, UnityEngine.Color.green);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
