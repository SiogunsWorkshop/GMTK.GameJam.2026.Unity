using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DelayedAbility : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<float> OnAbilityDelayStarted { get; private set; } = new();
    [field: SerializeField] public UnityEvent<float> OnAbilityDelayUpdated { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnAbilityTriggered { get; private set; } = new();

    [SerializeField] private float _delay = 1f;

    private bool IsDelayActive;
    private CancellationTokenSource _delayCTS;

    private void OnDestroy()
    {
        UniTaskTools.KillToken(ref _delayCTS);
    }

    public void TriggerAbility()
    {
        if (IsDelayActive) return;

        UniTaskTools.RenewToken(ref _delayCTS);
        HandleAbilityDelay(_delayCTS.Token).Forget();
    }

    private async UniTaskVoid HandleAbilityDelay(CancellationToken token)
    {
        IsDelayActive = true;
        OnAbilityDelayStarted.Invoke(_delay);

        float timeLeft = _delay;

        while (timeLeft > 0f && !token.IsCancellationRequested)
        {
            var normalizedProgress = Mathf.Clamp01(timeLeft / _delay);
            OnAbilityDelayUpdated.Invoke(normalizedProgress);
            timeLeft -= Time.deltaTime;
            await UniTask.Yield(PlayerLoopTiming.Update);
        }

        if (!token.IsCancellationRequested)
            OnAbilityTriggered.Invoke();

        IsDelayActive = false;
    }
}
