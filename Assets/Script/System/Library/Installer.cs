using Zenject;
using Presenter;

public class Installer : MonoInstaller
{
    /// <summary>
    /// Zenject DI container��Bind
    /// </summary>
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PoiPresenter>().AsCached();
        Container.BindInterfacesAndSelfTo<StagePresenter>().AsCached();
        Container.Bind<GameConfig>().AsSingle();
    }
}
