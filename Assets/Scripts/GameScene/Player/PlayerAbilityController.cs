using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerAbilityController : MonoBehaviour, InputMap.IAbilitiesActions
{
    [SerializeField] private DelayedAbility _dash;

    [Inject] private readonly InputMap _inputMap;

    private void OnEnable()
    {
        _inputMap.Abilities.SetCallbacks(this);
        _inputMap.Abilities.Enable();
    }

    private void OnDisable()
    {
        _inputMap.Abilities.Disable();
        _inputMap.Abilities.SetCallbacks(null);
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
        if (!context.performed) return;
        _dash.TriggerAbility();
    }
}
