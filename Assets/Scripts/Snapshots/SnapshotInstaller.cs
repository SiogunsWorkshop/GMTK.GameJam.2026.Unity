using Zenject;

public class SnapshotInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlaythroughSummarySnapshotService>().AsSingle();
    }
}