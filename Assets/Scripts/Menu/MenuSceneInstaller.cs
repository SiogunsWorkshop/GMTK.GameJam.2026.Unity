using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class MenuSceneInstaller : MonoInstaller
{
    [SerializeField, Required] private MenuSceneManager _menuSceneManager;

    public override void InstallBindings()
    {
        if (_menuSceneManager)
        {
            Container.Bind<MenuSceneManager>().FromInstance(_menuSceneManager).AsSingle();
        }
        else
        {
            Debug.LogWarning("MenuSceneManager is not assigned in the inspector. Attempting to bind from hierarchy.");
            Container.Bind<MenuSceneManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
