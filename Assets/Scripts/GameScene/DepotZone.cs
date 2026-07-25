using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider2D))]
public class DepotZone : MonoBehaviour
{
    [Inject] private readonly GameSceneManager _gameSceneManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent(out UniversalBouncer cargo))
            return;

        _gameSceneManager.HandleDepotDelivery();
        Destroy(cargo.gameObject);
    }
}
