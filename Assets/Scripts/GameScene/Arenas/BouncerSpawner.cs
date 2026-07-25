using Sirenix.OdinInspector;
using UnityEngine;

public class BouncerSpawner : MonoBehaviour
{
    [SerializeField, Required] private GameObject _bouncerPrefab;

    private GameObject _currentBouncer;

    private void OnEnable()
    {
        SpawnBouncer();
    }

    private void OnDisable()
    {
        KillBouncer();
    }

    public void SpawnBouncer()
    {
        if (_bouncerPrefab != null)
        {
            _currentBouncer = Instantiate(_bouncerPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("Bouncer prefab is not assigned in the inspector.");
        }
    }

    public void KillBouncer()
    {
        if (_currentBouncer != null)
        {
            Destroy(_currentBouncer);
            _currentBouncer = null;
        }
    }
}
