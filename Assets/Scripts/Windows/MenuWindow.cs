using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class MenuWindow : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnSettingsButtonClickedEvent { get; private set; } = new();

    [SerializeField, ReadOnly, ShowInInspector] private CanvasGroup _canvasGroup;
    [SerializeField, ReadOnly, ShowInInspector] private RectTransform _rectTransform;

    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private Tween _slideTween;
    private Canvas _canvas;

    [Inject] private readonly ProjectManager _projectManager;

    private const float SLIDE_DURATION = 0.5f;

    private void Awake()
    {
        _canvasGroup.alpha = 1f;
        _canvas = GetComponentInParent<Canvas>();

        _slideTween = _rectTransform
            .DOAnchorPosX(-_canvas.pixelRect.width, SLIDE_DURATION)
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false);
    }

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayButtonClicked);
        _settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        _quitButton.onClick.AddListener(OnQuitButtonClicked);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayButtonClicked);
        _settingsButton.onClick.RemoveListener(OnSettingsButtonClicked);
        _quitButton.onClick.RemoveListener(OnQuitButtonClicked);
    }

    [ContextMenu("Soft Reset")]
    private void Reset()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public void SlideIntoView()
    {
        if (_slideTween.IsPlaying())
        {
            Debug.LogWarning("Slide tween is already playing. Ignoring SlideIntoView call.");
            return;
        }

        _slideTween.PlayBackwards();
    }

    public void SlideOutOfView()
    {
        if (_slideTween.IsPlaying())
        {
            Debug.LogWarning("Slide tween is already playing. Ignoring SlideOutOfView call.");
            return;
        }

        _slideTween.Restart();
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void OnSettingsButtonClicked()
    {
        SlideOutOfView();
        OnSettingsButtonClickedEvent.Invoke();
    }

    private void OnPlayButtonClicked()
    {
        _canvasGroup.interactable = false;
        _projectManager.LoadScene(ProjectManager.SceneName.GameScene);
    }


}
