using Core.Factories;
using Core.Runtime;
using Core.Services;
using Core.Services.WordContainers;
using Core.Systems;
using Core.Systems.WordContainer;
using UI.Flow;
using UI.Loaders;
using UI.Services;
using Zenject;

namespace Architecture.DI
{
    public sealed class UIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallFactories();
            BindServices();
            BindWindowLoader();
            BindFlow();
        }

        private void InstallFactories()
        {
            Container.BindInterfacesTo<UIWordContainerDependenciesFactory>().AsSingle();
            Container.BindInterfacesTo<UIClusterFactory>().AsSingle();
            Container.BindInterfacesTo<UIWordContainerFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<UIClustersService>().AsSingle();
            Container.BindInterfacesTo<WordContainersService>().AsSingle();
            Container.Bind<IDropPlacementHelper>().To<BottomDropPlacementHandler>().AsSingle();
            Container.Bind<IContainerDropPlacementHelper>().To<ContainerDropPlacementHelper>().AsSingle();
        }

        private void BindWindowLoader()
        {
            Container.Bind<IUIWindowLoader>().To<AddressablesWindowLoader>().AsSingle();
        }

        private void BindFlow()
        {
            Container.BindInterfacesAndSelfTo<UIFlowManager>().AsSingle().Lazy();
        }
    }
}