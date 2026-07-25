using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class UniversalBouncer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Collider2D _collider;
    [SerializeField] private float _force = 1;

    private CancellationTokenSource _handleBouncer;
    private Vector2 _direction;

    private void Reset()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        _rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        //_rigidbody.sharedMaterial = material here
        //_rigidBody.linearDamping = 1; //when fixed
        _rigidBody.angularDamping = 0;
    }

    private void Start()
    {
        _direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        _rigidBody.AddForce(_direction * _force, ForceMode2D.Impulse);

        UniTaskTools.RenewToken(ref _handleBouncer);
        HandleBouncer(_handleBouncer.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleBouncer);
    }

    private async UniTask HandleBouncer(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {
            //if(_rigidBody.linearVelocity.magnitude < 5)
            //    _rigidBody.AddForce(_direction * _force, ForceMode2D.Force);
            Debug.DrawRay(gameObject.transform.position, _direction, UnityEngine.Color.green);
            Debug.DrawRay(gameObject.transform.position, _rigidBody.linearVelocity, UnityEngine.Color.red);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 incoming = -collision.relativeVelocity;
        Vector2 normal = collision.contacts[0].normal;
        _direction = Vector2.Reflect(incoming, normal).normalized;
    }
}
