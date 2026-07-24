using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
public class SettingsWindow : MonoBehaviour
{
    [field: SerializeField] public UnityEvent OnBackButtonClickedEvent { get; private set; } = new();

    [SerializeField, ReadOnly, ShowInInspector] private CanvasGroup _canvasGroup;
    [SerializeField, ReadOnly, ShowInInspector] private RectTransform _rectTransform;

    [SerializeField] private Button _backButton;

    private Tween _slideTween;

    private const float SLIDE_DURATION = 0.5f;

    private void Awake()
    {
        _canvasGroup.alpha = 1f;

        _slideTween = _rectTransform
            .DOAnchorPosX(0, SLIDE_DURATION)
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false);
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
    }

    [ContextMenu("Soft Reset")]
    private void Reset()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    public void SlideIntoView()
    {
        if (_slideTween.IsPlaying())
        {
            Debug.LogWarning("SlideIntoView called while slide tween is already playing. Ignoring the call.");
            return;
        }

        _slideTween.Restart();
    }

    public void SlideOutOfView()
    {
        if (_slideTween.IsPlaying())
        {
            Debug.LogWarning("SlideOutOfView called while slide tween is already playing. Ignoring the call.");
            return;
        }

        _slideTween.PlayBackwards();
    }

    private void OnBackButtonClicked()
    {
        SlideOutOfView();
        OnBackButtonClickedEvent.Invoke();
    }
}
