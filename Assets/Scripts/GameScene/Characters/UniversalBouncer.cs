using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class UniversalBouncer : MonoBehaviour
{
    public Rigidbody2D Rigidbody => _rigidBody;
    public Collider2D Collider => _collider;

    [SerializeField] private Rigidbody2D _rigidBody;
    [SerializeField] private Collider2D _collider;

    private CancellationTokenSource _handleBouncer;

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
            Debug.DrawRay(gameObject.transform.position, _rigidBody.linearVelocity, UnityEngine.Color.red);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
}
