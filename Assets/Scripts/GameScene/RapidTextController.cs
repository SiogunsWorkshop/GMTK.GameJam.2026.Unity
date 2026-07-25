using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class RapidTextController : MonoBehaviour
{
    [field: SerializeField] public UnityEvent<string> OnTextDisplayed { get; private set; } = new();

    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _displayDuration = 0.3f;

    private CancellationTokenSource _cts;

    private void Awake()
    {
        _text.text = string.Empty;
    }

    public void RapidFireText(string text)
    {
        string[] words = text.Split(' ');

        UniTaskTools.RenewToken(ref _cts);
        DisplayTextAsync(words, _cts.Token).Forget();
    }

    private async UniTaskVoid DisplayTextAsync(string[] words, CancellationToken token)
    {
        foreach (string word in words)
        {
            _text.text = word;
            OnTextDisplayed.Invoke(word);
            await UniTask.Delay(System.TimeSpan.FromSeconds(_displayDuration), cancellationToken: token);
        }

        _text.text = string.Empty;
    }
}

