using Input;
using UnityEngine;
using Zenject;

public class InputInstaller : Installer
{
    public override void InstallBindings()
    {
        Container.Bind<InputMap>().AsSingle().NonLazy();
    }
}