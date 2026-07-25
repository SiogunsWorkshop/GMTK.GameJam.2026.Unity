using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class UniversalBouncer : MonoBehaviour
{
    public UnityEvent<UniversalBouncer, Collision2D> OnBounced { get; private set; } = new();

    public Rigidbody2D Rigidbody => _rigidBody;
    public Collider2D Collider => _collider;
    public Team Team => _team;

    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Team _team = Team.None;

    private CancellationTokenSource _handleBouncer;

    private void Reset()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rigidBody.sharedMaterial = Resources.Load<PhysicsMaterial2D>("FunnyBouncy");
        _rigidBody.linearDamping = 1;
        _rigidBody.angularDamping = 0;
        _rigidBody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleBouncer);
        HandleBouncer(_handleBouncer.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleBouncer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBounced.Invoke(this, collision);
    }

    private async UniTask HandleBouncer(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            Debug.DrawRay(gameObject.transform.position, _rigidBody.linearVelocity.normalized * FloatTools.RemapClamped(_rigidBody.linearVelocity.magnitude, 0f, 10f, 0.3f, 1.5f), UnityEngine.Color.red);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
