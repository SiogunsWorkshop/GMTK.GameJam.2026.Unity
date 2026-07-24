using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ProjectManager : MonoBehaviour
{
    [Inject] private readonly LoadingWindow _loadingWindow;

    public enum SceneName
    {
        MenuScene,
        GameScene,
        SummaryScene
    }

    public void LoadScene(SceneName sceneName) =>
        HandlePlaySceneTransitionAsync(sceneName.ToString()).Forget();

    private async UniTaskVoid HandlePlaySceneTransitionAsync(string sceneName)
    {
        var loadScene = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        loadScene.allowSceneActivation = false;

        UniTaskCompletionSource loadingScreenFadeInTCS = new();
        _loadingWindow.Show(onComplete: () => loadingScreenFadeInTCS.TrySetResult());
        await loadingScreenFadeInTCS.Task;

        loadScene.allowSceneActivation = true;
    }
}
