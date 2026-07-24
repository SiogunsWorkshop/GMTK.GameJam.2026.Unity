using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class LoadingWindow : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;

    private Tween _fadeTween;

    public const float DEFAULT_FADE_DURATION = 0.6f;

    private void Reset()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(float onsetDuration = DEFAULT_FADE_DURATION)
    {
        _fadeTween?.Kill();
        _canvasGroup.alpha = 0f;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;

        if (onsetDuration <= 0f)
        {
            _canvasGroup.alpha = 1f;
            return;
        }
        _fadeTween = _canvasGroup
            .DOFade(1f, onsetDuration)
            .OnComplete(() =>
            {
                _canvasGroup.alpha = 1f;
            }).Play();
    }

    public void Hide(float onsetDuration = DEFAULT_FADE_DURATION)
    {
        _fadeTween?.Kill();
        _canvasGroup.alpha = 1f;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;

        if (onsetDuration <= 0f)
        {
            _canvasGroup.alpha = 0f;
            return;
        }
        _fadeTween = _canvasGroup
            .DOFade(0f, onsetDuration)
            .OnComplete(() =>
            {
                _canvasGroup.alpha = 0f;
            }).Play();
    }
}
