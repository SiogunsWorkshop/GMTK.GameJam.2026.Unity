using UnityEngine;
using Zenject;

public class GameSceneManager : MonoBehaviour
{
    [Inject] private readonly LoadingWindow _loadingWindow;

    private void Awake()
    {
        _loadingWindow.Hide();
    }
}
