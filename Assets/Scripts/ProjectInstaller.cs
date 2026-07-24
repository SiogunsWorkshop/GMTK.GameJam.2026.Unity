using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private LoadingWindow _loadingWindow;

    public override void InstallBindings()
    {
        Container.Install<InputInstaller>();

        Container.Bind<LoadingWindow>().FromInstance(_loadingWindow).AsSingle();
        Container.Bind<ProjectManager>().FromComponentInHierarchy().AsSingle();
    }
}