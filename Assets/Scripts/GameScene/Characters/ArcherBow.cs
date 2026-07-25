using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using System;
using System.Threading;
using UnityEngine;

public class ArcherBow : MonoBehaviour
{
    [SerializeField] private EnemyMovement _archer;
    [SerializeField] private float _angleChangeOnBounce = 25;
    [SerializeField] private bool _clockwise = true;
    [SerializeField] private float _shootCooldown = 2f;

    private CancellationTokenSource _handleBow;
    private float _bowAngle = 0f;

    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleBow);
        _archer.Bouncer.OnBounced.AddListener(OnBounced);

        HandleBow(_handleBow.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleBow);
        _archer.Bouncer.OnBounced.RemoveListener(OnBounced);
    }

    private async UniTask HandleBow(CancellationToken token)
    {
        await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, _archer.MovementDirection);
        _bowAngle = gameObject.transform.rotation.eulerAngles.z;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, _bowAngle);
        await UniTask.Delay(TimeSpan.FromSeconds(1), delayType: DelayType.DeltaTime);

        while (token.IsCancellationRequested == false)
        {
            Shoot();
            await UniTask.Delay(TimeSpan.FromSeconds(_shootCooldown), delayType: DelayType.DeltaTime);
        }
    }
    private void OnBounced(UniversalBouncer bouncer, Collision2D collision)
    {
        Rotate();
    }
    private void Rotate()
    {
        _bowAngle += _clockwise == true ? -_angleChangeOnBounce : _angleChangeOnBounce;
        _bowAngle = _bowAngle % 360;
        gameObject.transform.rotation = Quaternion.Euler(0,0,_bowAngle);
    }
    private void Shoot()
    {

    }
}
