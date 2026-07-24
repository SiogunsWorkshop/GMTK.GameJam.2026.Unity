using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FMOD;
using FMOD.Studio;
using FMODUnity;
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
    [SerializeField] private VolumeSettings[] _volumeSettings;

    private Dictionary<VolumeSettings.BusType, VolumeSettings> _volumeSettingsDictionary;

    private Tween _slideTween;

    private const float SLIDE_DURATION = 0.5f;

    [Serializable]
    private struct VolumeSettings
    {
        public Slider Slider;
        public BusType Type;
        public string BusPath;
        public readonly Bus Bus => RuntimeManager.GetBus(BusPath);
        public readonly bool SetVolume(float volume)
        {
            var result = Bus.setVolume(volume);
            if (result != RESULT.OK)
            {
                UnityEngine.Debug.LogError($"Failed to set volume for bus '{BusPath}'. FMOD result: {result}");
                return false;
            }
            return true;
        }

        public enum BusType
        {
            Master,
            SFX,
            Music
        }
    }

    private void Awake()
    {
        _volumeSettingsDictionary = _volumeSettings.ToDictionary(vs => vs.Type, vs => vs);
        _canvasGroup.alpha = 1f;

        _slideTween = _rectTransform
            .DOAnchorPosX(0, SLIDE_DURATION)
            .SetEase(Ease.InOutSine)
            .SetAutoKill(false);
    }

    private void OnEnable()
    {
        _backButton.onClick.AddListener(OnBackButtonClicked);
        _volumeSettingsDictionary[VolumeSettings.BusType.SFX].Slider.onValueChanged.AddListener(OnSFXVolumeChanged);
        _volumeSettingsDictionary[VolumeSettings.BusType.Music].Slider.onValueChanged.AddListener(OnMusicVolumeChanged);
        _volumeSettingsDictionary[VolumeSettings.BusType.Master].Slider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    private void OnDisable()
    {
        _backButton.onClick.RemoveListener(OnBackButtonClicked);
        _volumeSettingsDictionary[VolumeSettings.BusType.SFX].Slider.onValueChanged.RemoveListener(OnSFXVolumeChanged);
        _volumeSettingsDictionary[VolumeSettings.BusType.Music].Slider.onValueChanged.RemoveListener(OnMusicVolumeChanged);
        _volumeSettingsDictionary[VolumeSettings.BusType.Master].Slider.onValueChanged.RemoveListener(OnMasterVolumeChanged);
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
            UnityEngine.Debug.LogWarning("SlideIntoView called while slide tween is already playing. Ignoring the call.");
            return;
        }

        _slideTween.Restart();
    }

    public void SlideOutOfView()
    {
        if (_slideTween.IsPlaying())
        {
            UnityEngine.Debug.LogWarning("SlideOutOfView called while slide tween is already playing. Ignoring the call.");
            return;
        }

        _slideTween.PlayBackwards();
    }

    private void OnBackButtonClicked()
    {
        SlideOutOfView();
        OnBackButtonClickedEvent.Invoke();
    }

    private void OnMusicVolumeChanged(float value)
    {
        _volumeSettingsDictionary[VolumeSettings.BusType.Music].SetVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        _volumeSettingsDictionary[VolumeSettings.BusType.SFX].SetVolume(value);
    }

    private void OnMasterVolumeChanged(float value)
    {
        _volumeSettingsDictionary[VolumeSettings.BusType.Master].SetVolume(value);
    }
}
