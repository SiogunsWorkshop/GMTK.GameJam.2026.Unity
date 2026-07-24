using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class SummaryWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private TMP_Text _scoreText;

    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _quitButton;

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
    }

    private void OnRestartButtonClicked()
    {
        _canvasGroup.interactable = false;
    }
}
