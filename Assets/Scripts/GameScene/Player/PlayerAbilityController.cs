using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerAbilityController : MonoBehaviour, InputMap.IAbilitiesActions
{
    [SerializeField] private DelayedAbility _dash;

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

    }

    private void OnDisable()
    {
        _inputMap.Abilities.Disable();
        _inputMap.Abilities.SetCallbacks(null);

        _dash.OnAbilityDelayUpdated.RemoveListener(_delayedAbilityCountdownDisplay.UpdateFill);
        _dash.OnAbilityDelayStarted.RemoveListener(_delayedAbilityCountdownDisplay.SetFullFill_Wrapper);
        _dash.OnAbilityTriggered.RemoveListener(_delayedAbilityCountdownDisplay.SetEmptyFill);
        _dash.OnAbilityTriggered.RemoveListener(ReleaseIsUsingAbilityFlag);
    }

    private void ReleaseIsUsingAbilityFlag()
    {
        _isUsingAbility = false;
    }

    public void OnAbility1(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnAbility2(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if (!context.performed || _isUsingAbility) return;

        _isUsingAbility = true;
        _dash.TriggerAbility();
    }
}
