using UnityEngine;
using UnityEngine.Events;

public class EnvironmentalHazard : MonoBehaviour
{
    [SerializeField] private UnityEvent _onHazardActivated = new();
    [SerializeField] private UnityEvent _onHazardDeactivated = new();

    private void OnEnable()
    {
        _onHazardActivated.Invoke();
    }

    private void OnDisable()
    {
        _onHazardDeactivated.Invoke();
    }
}
