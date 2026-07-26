using Cysharp.Threading.Tasks;
using DG.Tweening.Core.Easing;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class ArcherBow : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnShoot { get; set; } = new();

    [SerializeField] private EnemyMovement _archer;
    [SerializeField] private GameObject _spawnPivot;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Arrow _arrowPrefab;
    [SerializeField] private float _angleChangeOnBounce = 25;
    [SerializeField] private bool _clockwise = true;
    [SerializeField] private float _shootCooldown = 2f;

    private CancellationTokenSource _handleBow;
    private float _bowAngle = 0f;

    private bool _isDisabled;
    private void Start()
    {
        UniTaskTools.RenewToken(ref _handleBow);
        _archer.Bouncer.OnBounced.AddListener(OnBounced);
        _archer.OnDisabled.AddListener(OnDisabled);
        _archer.OnDisableEnd.AddListener(OnEnabledAgain);

        HandleBow(_handleBow.Token).Forget();
    }
    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _handleBow);
        _archer.Bouncer.OnBounced.RemoveListener(OnBounced);
        _archer.OnDisabled.RemoveListener(OnDisabled);
        _archer.OnDisableEnd.RemoveListener(OnEnabledAgain);
    }

    private async UniTask HandleBow(CancellationToken token)
    {
        await UniTask.Yield(PlayerLoopTiming.PostLateUpdate);
        gameObject.transform.rotation = Quaternion.FromToRotation(Vector2.up, _archer.MovementDirection);
        _bowAngle = gameObject.transform.rotation.eulerAngles.z;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, _bowAngle);
        _spawnPivot.transform.rotation = Quaternion.Euler(0, 0, _bowAngle);
        await UniTask.Delay(TimeSpan.FromSeconds(1), delayType: DelayType.DeltaTime);

        while (token.IsCancellationRequested == false)
        {
            if (_isDisabled == false)
                Shoot();
            await UniTask.Delay(TimeSpan.FromSeconds(_shootCooldown), delayType: DelayType.DeltaTime);
        }
    }
    private void OnBounced(UniversalBouncer bouncer, Collision2D collision)
    {
        Rotate();
        _spawnPivot.transform.rotation = Quaternion.Euler(0, 0, _bowAngle);
    }
    private void Rotate()
    {
        _bowAngle += _clockwise == false ? _angleChangeOnBounce : -_angleChangeOnBounce;
        _bowAngle = _bowAngle % 360;
        gameObject.transform.rotation = Quaternion.Euler(0, 0, _bowAngle);
    }
    private void Shoot()
    {
        var arrow = Instantiate(_arrowPrefab, _spawnPoint.position, Quaternion.identity);

        float radians = _bowAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(-Mathf.Sin(radians), Mathf.Cos(radians));

        arrow.Spawn(_archer.Bouncer.Team, direction);

        OnShoot.Invoke();
    }

    public void OnDisabled(float time)
    {
        _isDisabled = true;
        ////////// disable visual effect here ///////////
    }
    public void OnEnabledAgain()
    {
        _isDisabled = false;
        ////////// Enable visual effect again ///////////
    }
}
