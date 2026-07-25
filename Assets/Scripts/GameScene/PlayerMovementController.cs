using Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Zenject;

public class PlayerMovementController : MonoBehaviour, InputMap.IMovementActions
{
    public readonly UnityEvent<Vector2> OnMoveInputStarted = new();
    public readonly UnityEvent<Vector2> OnMoveInputPerformed = new();
    public readonly UnityEvent<Vector2> OnMoveInputCanceled = new();

    [Inject] private readonly InputMap _inputMap;

    private void OnEnable()
    {
        _inputMap.Movement.SetCallbacks(this);
        _inputMap.Movement.Enable();
    }

    private void OnDisable()
    {
        _inputMap.Movement.Disable();
        _inputMap.Movement.SetCallbacks(null);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        if (context.started)
            OnMoveInputStarted.Invoke(input);
        else if (context.performed)
            OnMoveInputPerformed.Invoke(input);
        else if (context.canceled)
            OnMoveInputCanceled.Invoke(input);
    }
}
