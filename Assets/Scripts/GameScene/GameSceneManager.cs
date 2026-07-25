using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnDepotDelivery { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnPlayerDeath { get; private set; } = new();

    public int ActiveCargoCount { get; set; }

    [SerializeField] private HealthComponent _playerHealthComponent;

    private int _score;

    [Inject] private readonly LoadingWindow _loadingWindow;
    [Inject] private readonly ProjectManager _projectManager;
    [Inject] private readonly PlaythroughSummarySnapshotService _playthroughSnapshotService;
    [Inject] private readonly ArenaController _arenaController;

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

    public void HandleDepotDelivery()
    {
        _score += 1;
        OnDepotDelivery.Invoke();

        if (ActiveCargoCount == 1)
        {
            _arenaController.RerollEnvironmentalHazards();
        }
    }

    private void HandlePlayerDeath()
    {
        HandlePlayerDeathAsync().Forget();
    }

    private async UniTaskVoid HandlePlayerDeathAsync()
    {
        await UniTask.Yield(); // This is to suppress the warning about async void methods

        var time = Time.timeSinceLevelLoad;

        _playthroughSnapshotService.SaveSnapshot((_score, time));
        _projectManager.LoadScene(ProjectManager.SceneName.SummaryScene);
    }
}
