using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class EnvironmentalHazard : MonoBehaviour
{
    [Min(1)] public int CargoCount = 1;

    [SerializeField] private UnityEvent _onHazardActivated = new();
    [SerializeField] private UnityEvent _onHazardDeactivated = new();

    [Inject] private readonly ArenaController _arenaController;

    private void OnEnable()
    {
        _onHazardActivated.Invoke();
        _arenaController.CargoCount += CargoCount;
    }

    private void OnDisable()
    {
        _onHazardDeactivated.Invoke();
    }
}
