using Assets.Scripts.Infrastructure;
using Zenject;

namespace Assets.Scripts.Architecture.DI
{
    public class GameInstaller : Installer<GameInstaller>
    {
        public override void InstallBindings() 
        {
            Container.Bind<AppStartup>().AsSingle().NonLazy();
        }
    }
}