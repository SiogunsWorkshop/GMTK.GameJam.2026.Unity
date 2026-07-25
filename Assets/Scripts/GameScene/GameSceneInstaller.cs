using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<GameSceneManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<PlayerMovementController>().FromComponentInHierarchy().AsSingle();
    }
}
