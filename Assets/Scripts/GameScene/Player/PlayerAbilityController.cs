using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerAbilityController : MonoBehaviour, InputMap.IAbilitiesActions
{
    [SerializeField] private DelayedAbility _dash;
    [SerializeField] private DelayedAbility _explode;
    [SerializeField] private DelayedAbility _attack;

    private bool _isUsingAbility;

    [Inject] private readonly InputMap _inputMap;
    [Inject] private readonly DelayedAbilityCountdownDisplay _delayedAbilityCountdownDisplay;

    private void OnEnable()
    {
        _inputMap.Abilities.SetCallbacks(this);
        _inputMap.Abilities.Enable();

        _dash.OnAbilityDelayUpdated.AddListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _dash.OnAbilityDelayStarted.AddListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _dash.OnAbilityTriggered.AddListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _dash.OnAbilityTriggered.AddListener(ReleaseIsUsingAbilityFlag);

        _explode.OnAbilityDelayUpdated.AddListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _explode.OnAbilityDelayStarted.AddListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _explode.OnAbilityTriggered.AddListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _explode.OnAbilityTriggered.AddListener(ReleaseIsUsingAbilityFlag);

        _attack.OnAbilityDelayUpdated.AddListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _attack.OnAbilityDelayStarted.AddListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _attack.OnAbilityTriggered.AddListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _attack.OnAbilityTriggered.AddListener(ReleaseIsUsingAbilityFlag);

    }

    private void OnDisable()
    {
        _inputMap.Abilities.Disable();
        _inputMap.Abilities.SetCallbacks(null);

        _dash.OnAbilityDelayUpdated.RemoveListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _dash.OnAbilityDelayStarted.RemoveListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _dash.OnAbilityTriggered.RemoveListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _dash.OnAbilityTriggered.RemoveListener(ReleaseIsUsingAbilityFlag);

        _explode.OnAbilityDelayUpdated.RemoveListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _explode.OnAbilityDelayStarted.RemoveListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _explode.OnAbilityTriggered.RemoveListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _explode.OnAbilityTriggered.RemoveListener(ReleaseIsUsingAbilityFlag);

        _attack.OnAbilityDelayUpdated.RemoveListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _attack.OnAbilityDelayStarted.RemoveListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _attack.OnAbilityTriggered.RemoveListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _attack.OnAbilityTriggered.RemoveListener(ReleaseIsUsingAbilityFlag);
    }

    private void ReleaseIsUsingAbilityFlag()
    {
        _isUsingAbility = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (!context.performed || _isUsingAbility) return;

        _isUsingAbility = true;
        _attack.TriggerAbility();
    }

    public void OnExplode(InputAction.CallbackContext context)
    {
        if (!context.performed || _isUsingAbility) return;

        _isUsingAbility = true;
        _explode.TriggerAbility();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || _isUsingAbility) return;

        _isUsingAbility = true;
        _dash.TriggerAbility();
    }
}
