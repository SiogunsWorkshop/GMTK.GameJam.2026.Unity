using UnityEngine;
using Zenject;

public class CargoComponent : MonoBehaviour
{
    [Inject] private readonly GameSceneManager _gameSceneManager;

    private void OnEnable()
    {
        _gameSceneManager.ActiveCargoCount++;
    }

    private void OnDisable()
    {
        _gameSceneManager.ActiveCargoCount--;
    }
}
