using UnityEngine;
using Zenject;

public class SummarySceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<SummarySceneManager>().FromComponentInHierarchy().AsSingle();
    }
}
