using UnityEngine;
using Zenject;

public class MenuSceneManager : MonoBehaviour
{
    [Inject] private readonly LoadingWindow _loadingWindow;

    private static bool _firstTime = true;

    private void Awake()
    {
        if (_firstTime)
        {
            _firstTime = false;
            _loadingWindow.Hide(0f);
        }
        else
        {
            _loadingWindow.Hide();
        }
    }
}
