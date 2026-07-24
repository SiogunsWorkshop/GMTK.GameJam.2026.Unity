using UnityEngine;
using Zenject;

public class SummarySceneManager : MonoBehaviour
{
    [Inject] private readonly LoadingWindow _loadingWindow;

    private void Awake()
    {
        _loadingWindow.Hide();
    }
}
