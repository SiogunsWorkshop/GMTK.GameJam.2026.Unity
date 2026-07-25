using System;
using Cysharp.Threading.Tasks;
using Input;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

[RequireComponent(typeof(UniversalBouncer))]
public class PlayerControlledMovement : MonoBehaviour
{
    [SerializeField] private UniversalBouncer _bouncer;
    [SerializeField] private float _force = 1;

    [Inject] private readonly PlayerMovementController _playerMovement;

    private Vector2 _movementInput;

    private void OnEnable()
    {
        _playerMovement.OnMoveInputPerformed.AddListener(OnMoveInputPerformed);
        _playerMovement.OnMoveInputCanceled.AddListener(OnMoveInputCanceled);
    }

    private void OnDisable()
    {
        _playerMovement.OnMoveInputPerformed.RemoveListener(OnMoveInputPerformed);
        _playerMovement.OnMoveInputCanceled.RemoveListener(OnMoveInputCanceled);
    }

    private void FixedUpdate()
    {
        if (_movementInput == Vector2.zero)
            return;

        _bouncer.Rigidbody.AddForce(_movementInput * _force, ForceMode2D.Force);
    }

    private void Reset()
    {
        _bouncer = GetComponent<UniversalBouncer>();
    }

    private void OnMoveInputCanceled(Vector2 arg0)
    {
        _movementInput = Vector2.zero;
    }

    private void OnMoveInputPerformed(Vector2 arg0)
    {
        _movementInput = arg0;
    }
}
