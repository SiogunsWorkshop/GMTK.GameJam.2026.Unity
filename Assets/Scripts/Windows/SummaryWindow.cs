using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
public class SummaryWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _quitButton;

    [Inject] private readonly ProjectManager _projectManager;
    [Inject] private readonly PlaythroughSummarySnapshotService _playthroughSummarySnapshotService;

    private void Awake()
    {
        UpdateTime();
        UpdateScore();
    }

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClicked);
        _menuButton.onClick.AddListener(OnMenuButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartButtonClicked);
        _menuButton.onClick.RemoveListener(OnMenuButtonClicked);
        _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }

    private void Reset()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void UpdateTime()
    {
        var snapshot = _playthroughSummarySnapshotService.ReadSnapshot();
        _timeText.text = $"{snapshot.Time:0.00}";
    }

    private void UpdateScore()
    {
        var snapshot = _playthroughSummarySnapshotService.ReadSnapshot();
        _scoreText.text = $"{snapshot.Score}";
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnMenuButtonClicked()
    {
        _canvasGroup.interactable = false;
        _projectManager.LoadScene(ProjectManager.SceneName.MenuScene);

    }

    private void OnRestartButtonClicked()
    {
        _canvasGroup.interactable = false;
        _projectManager.LoadScene(ProjectManager.SceneName.GameScene);
    }
}
