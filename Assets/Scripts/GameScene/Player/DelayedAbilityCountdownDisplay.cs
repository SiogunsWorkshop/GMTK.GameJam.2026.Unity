using UnityEngine;
using UnityEngine.UI;

public class DelayedAbilityCountdownDisplay : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private void Awake()
    {
        SetEmptyFill();
    }

    public void SetEmptyFill() => UpdateFill(0f);

    public void SetFullFill_Wrapper(float _) => SetFullFill();
    public void SetFullFill() => UpdateFill(1f);

    public void UpdateFill(float fillAmount)
    {
        _fillImage.fillAmount = fillAmount;
    }
}
