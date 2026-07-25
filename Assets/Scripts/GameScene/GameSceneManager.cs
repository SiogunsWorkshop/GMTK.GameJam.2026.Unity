using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnPlayerDeath { get; private set; } = new();

    [SerializeField] private HealthComponent _playerHealthComponent;

    [Inject] private readonly LoadingWindow _loadingWindow;

    private void Awake()
    {
        _loadingWindow.Hide();
    }

    private void OnEnable()
    {
        _playerHealthComponent.OnDeath.AddListener(HandlePlayerDeath);
    }

    private void OnDisable()
    {
        _playerHealthComponent.OnDeath.RemoveListener(HandlePlayerDeath);
    }

    public void HandlePlayerDeath()
    {
        Debug.Log("Player has died. Handle game over logic here.");
    }
}
