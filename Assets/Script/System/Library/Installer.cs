using Zenject;
using Presenter;

public class Installer : MonoInstaller
{
    /// <summary>
    /// Zenject DI containerªË«Ð«¤«ó«É
    /// </summary>
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<PoiPresenter>().AsCached();
        Container.BindInterfacesAndSelfTo<StagePresenter>().AsCached();
        Container.Bind<GameConfig>().AsSingle();
        Container.BindInterfacesAndSelfTo<PoiPowerUpManager>().AsCached();
    }
}
