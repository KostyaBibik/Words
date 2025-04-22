using Core.Factories;
using Core.Services;
using Core.Services.WordContainers;
using Gameplay.Utils;
using UI.Core;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Validation;
using UI.Loading;
using UI.Victory;
using UnityEngine;
using Zenject;

namespace Architecture.DI
{
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _uiManagerPrefab;

        public override void InstallBindings()
        {
            InstallFactories();

            BindServices();
            
            CreateAndBindUIManager();

            BindWindows();

            BindValidation();
            
            BindFlow();
        }

        private void InstallFactories()
        {
            Container.BindInterfacesTo<UIClusterFactory>().AsSingle();
            Container.BindInterfacesTo<UIWordContainerFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<UIClustersService>().AsSingle();
            Container.BindInterfacesTo<WordContainersService>().AsSingle();
        }

        private void CreateAndBindUIManager()
        {
            var uiManager = Container.InstantiatePrefab(_uiManagerPrefab);
            Container.Bind<UIManager>().FromComponentOn(uiManager).AsSingle();
        }

        private void BindWindows()
        {
            Container.BindPresenterWithView<UILoadingPresenter, UILoadingView>();
            Container.BindPresenterWithView<UIErrorLoadingPresenter, UIErrorLoadingView>();
            Container.BindPresenterWithView<UIMainMenuPresenter, UIMainMenuView>();
            Container.BindPresenterWithView<UIClustersPanelPresenter, UIClustersPanelView>();
            Container.BindPresenterWithView<UIWordGridPresenter, UIWordGridView>();
            Container.BindPresenterWithView<UIGameplayPresenter, UIGameplayView>();
            Container.BindPresenterWithView<UIVictoryPresenter, UIVictoryView>();
        }

        private void BindValidation()
        {
            Container.BindPresenterWithView<UIValidationButtonPresenter, UIValidationButtonView>();
        }

        private void BindFlow()
        {
            Container.BindInterfacesAndSelfTo<UIFlowManager>().AsSingle();
        }
    }
}