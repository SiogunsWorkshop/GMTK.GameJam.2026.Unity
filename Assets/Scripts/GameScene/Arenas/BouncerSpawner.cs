using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class BouncerSpawner : MonoBehaviour
{
    [SerializeField, Required] private GameObject _bouncerPrefab;

    private GameObject _currentBouncer;
    private Tween _spawnTween;
    private Tween _despawnTween;
    private const float SPAWN_SCALE_DURATION = 0.5f;

    private void OnEnable()
    {
        SpawnBouncer();
    }

    private void OnDisable()
    {
        KillBouncer(false);
    }

    private void OnDestroy()
    {
        KillActiveTweens();
    }

    public void SpawnBouncer()
    {
        KillActiveTweens();

        if (_currentBouncer)
        {
            Destroy(_currentBouncer);
            _currentBouncer = null;
        }

        if (_bouncerPrefab != null)
        {
            _currentBouncer = Instantiate(_bouncerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Bouncer prefab is not assigned in the inspector.");
        }
        if (!_currentBouncer)
        {
            Debug.LogWarning("Bouncer failed to spawn.");
            return;
        }

        _currentBouncer.transform.localScale = Vector3.zero;
        _spawnTween = _currentBouncer.transform
            .DOScale(Vector3.one, SPAWN_SCALE_DURATION)
            .SetEase(Ease.OutBack)
            .SetLink(_currentBouncer, LinkBehaviour.KillOnDestroy)
            .OnKill(() => _spawnTween = null)
            .Play();
    }

    public void KillBouncer(bool animate = true)
    {
        if (_spawnTween != null && _spawnTween.IsActive())
        {
            _spawnTween.Kill();
        }

        if (!_currentBouncer) return;
        var bouncerToKill = _currentBouncer;
        _currentBouncer = null;

        if (!animate || !isActiveAndEnabled || !bouncerToKill.activeInHierarchy)
        {
            Destroy(bouncerToKill);
            return;
        }

        // Tween to zero during normal gameplay; skip this during teardown.
        _despawnTween = bouncerToKill.transform
            .DOScale(Vector3.zero, SPAWN_SCALE_DURATION)
            .SetEase(Ease.InBack)
            .SetLink(bouncerToKill, LinkBehaviour.KillOnDestroy)
            .OnKill(() => _despawnTween = null)
            .OnComplete(() =>
            {
                Destroy(bouncerToKill);
            })
            .Play();
    }

    private void KillActiveTweens()
    {
        if (_spawnTween != null && _spawnTween.IsActive())
        {
            _spawnTween.Kill();
        }

        if (_despawnTween != null && _despawnTween.IsActive())
        {
            _despawnTween.Kill();
        }
    }
}
