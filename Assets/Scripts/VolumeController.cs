using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class VolumeController : MonoBehaviour
{
    [SerializeField] private Volume _volume;

    [SerializeField] private float _playerDamagedEffectDuration = 0.45f;
    [SerializeField] private float _rapidDisplayEffectDuration = 0.2f;
    [SerializeField] private AnimationCurve chromaticAberrationPunchCurve = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField] private HealthComponent _playerHealth;

    private VolumeProfile RuntimeProfile => _volume.profile;
    private Tween _chromaticAberrationTween;
    private Tween _contrastTween;

    private float _ogChromaticAberrationIntensity;
    private ChromaticAberration _chromaticAberration;
    private float _ogContrastIntensity;
    private ColorAdjustments _colorAdjustments;

    private void Awake()
    {
        if (_volume == null)
            throw new System.Exception("Volume is not assigned in the inspector.");

        if (_playerHealth == null)
            throw new System.Exception("Player Health Component is not assigned in the inspector.");

        if (RuntimeProfile.TryGet<ChromaticAberration>(out var chromaticAberration))
        {
            _chromaticAberration = chromaticAberration;
            _ogChromaticAberrationIntensity = chromaticAberration.intensity.value;
        }
        else
        {
            Debug.LogError("Chromatic Aberration effect is not found in the Volume profile.");
        }

        if (RuntimeProfile.TryGet<ColorAdjustments>(out var colorAdjustments))
        {
            _colorAdjustments = colorAdjustments;
            _ogContrastIntensity = colorAdjustments.contrast.value;
        }
        else
        {
            Debug.LogError("Color Adjustments effect is not found in the Volume profile.");
        }
    }

    private void OnEnable()
    {
        _playerHealth.OnDamaged.AddListener(PunchChromaticAberration);
        _playerHealth.OnDamaged.AddListener(PunchContrast);

        Explode.OnAnyExplode.AddListener(PunchChromaticAberration);
        Explode.OnAnyExplode.AddListener(PunchContrast);
    }

    private void OnDisable()
    {
        _playerHealth.OnDamaged.RemoveListener(PunchChromaticAberration);
        _playerHealth.OnDamaged.RemoveListener(PunchContrast);

        Explode.OnAnyExplode.AddListener(PunchChromaticAberration);
        Explode.OnAnyExplode.AddListener(PunchContrast);
    }

    private void Reset()
    {
        _volume = GetComponent<Volume>();
    }

    [ContextMenu("Punch Chromatic Aberration")]
    public void PunchChromaticAberration() => PunchChromaticAberration(_playerDamagedEffectDuration, 1f);
    public void PunchChromaticAberration(float duration, float intensity)
    {
        _chromaticAberrationTween?.Kill();
        _chromaticAberrationTween = DOVirtual.Float(0, intensity, duration, value =>
        {
            _chromaticAberration.intensity.value = value;
        }).SetEase(chromaticAberrationPunchCurve).OnComplete(() =>
        {
            _chromaticAberration.intensity.value = _ogChromaticAberrationIntensity;
        }).Play();
    }

    [ContextMenu("Punch Contrast")]
    public void PunchContrast() => PunchContrast(_playerDamagedEffectDuration, 50f);
    public void PunchContrast(float intensity) => PunchContrast(_rapidDisplayEffectDuration, intensity);
    public void PunchContrast(float duration, float intensity)
    {
        _contrastTween?.Kill();
        _contrastTween = DOVirtual.Float(0, intensity, duration, value =>
        {
            _colorAdjustments.contrast.value = value;
        }).SetEase(chromaticAberrationPunchCurve).OnComplete(() =>
        {
            _colorAdjustments.contrast.value = _ogContrastIntensity;
        }).Play();
    }
}
