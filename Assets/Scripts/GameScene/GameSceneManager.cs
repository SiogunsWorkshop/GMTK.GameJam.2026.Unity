using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnDepotDelivery { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnPlayerDeath { get; private set; } = new();

    [SerializeField] private HealthComponent _playerHealthComponent;

    [SerializeField]
    private string[] _levelClearedMessages = new string[]
    {
        "Level Cleared",
        "Good Job",
        "Well Done",
        "Keep Going",
        "Cargo Delivered",
        "Cargo Secured",
    };

    [SerializeField]
    private string[] _cargoCountMessages = new string[]
    {
        "Cargo Remaining",
        "Cargo Left",
        "Cargo To Go",
        "Cargo Still Out There",
    };

    private int _score;

    [Inject] private readonly LoadingWindow _loadingWindow;
    [Inject] private readonly ProjectManager _projectManager;
    [Inject] private readonly PlaythroughSummarySnapshotService _playthroughSnapshotService;
    [Inject] private readonly ArenaController _arenaController;
    [Inject] private readonly RapidTextController _rapidTextController;

    private const string FALLBACK_LEVEL_CLEARED_MESSAGE = "CARGO DELIVERED";
    private const string CARGO_REMAINING_COUNT_MESSAGE = "MORE";

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
        _arenaController.CargoCount--;

        if (_arenaController.CargoCount <= 0)
        {
            _arenaController.RerollEnvironmentalHazards();

            string message = _levelClearedMessages.Length == 0 ? FALLBACK_LEVEL_CLEARED_MESSAGE : _levelClearedMessages[Random.Range(0, _levelClearedMessages.Length)];
            _rapidTextController.RapidFireText(message);
        }
        else
        {
            string cargoCountMessage = _cargoCountMessages.Length == 0 ? CARGO_REMAINING_COUNT_MESSAGE : _cargoCountMessages[Random.Range(0, _cargoCountMessages.Length)];
            string message = $"{_arenaController.CargoCount} {cargoCountMessage}";
            _rapidTextController.RapidFireText(message);
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
