using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using System.Threading;
using UnityEngine;

public class ArcherBow : MonoBehaviour
{
    [SerializeField] private EnemyMovement _archer;
    [SerializeField] private float _angleChangeOnBounce = 25;
    [SerializeField] private bool _clockwise = true;

    private CancellationTokenSource _handleBow;
    private float _bowAngle = 0f;

    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleBow);
        _archer.Bouncer.OnBounced.AddListener(OnBounced);
        _archer.Bouncer.OnBounced.RemoveListener(OnBounced);

        HandleBow(_handleBow.Token).Forget();
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, _archer.MovementDirection);
        _bowAngle = gameObject.transform.rotation.eulerAngles.z;
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleBow);
        _archer.Bouncer.OnBounced.RemoveListener(OnBounced);
    }

    private async UniTask HandleBow(CancellationToken token)
    {
        while (token.IsCancellationRequested == false)
        {

            await UniTask.Yield(PlayerLoopTiming.Update);
        }
    }
    private void OnBounced(UniversalBouncer bouncer, Collision2D collision)
    {
        Rotate();
    }
    private void Rotate()
    {
        _bowAngle += _clockwise == true ? _angleChangeOnBounce : -_angleChangeOnBounce;
        gameObject.transform.rotation = Quaternion.Euler(0,0,_bowAngle);
    }
}
