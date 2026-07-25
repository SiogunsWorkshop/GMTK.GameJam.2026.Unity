using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnPlayerDeath { get; private set; } = new();

    [SerializeField] private HealthComponent _playerHealthComponent;

    [Inject] private readonly LoadingWindow _loadingWindow;
    [Inject] private readonly ProjectManager _projectManager;
    [Inject] private readonly PlaythroughSummarySnapshotService _playthroughSnapshotService;

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

    private void HandlePlayerDeath()
    {
        HandlePlayerDeathAsync().Forget();
    }

    private async UniTaskVoid HandlePlayerDeathAsync()
    {
        await UniTask.Yield(); // This is to suppress the warning about async void methods

        var time = Time.timeSinceLevelLoad;
        var score = 0; // TODO: Get score from score manager

        _playthroughSnapshotService.SaveSnapshot((score, time));
        _projectManager.LoadScene(ProjectManager.SceneName.SummaryScene);
    }
}
