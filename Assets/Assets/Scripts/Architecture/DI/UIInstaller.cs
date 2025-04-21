using Core.Factories;
using Core.Services;
using Core.Services.Validation;
using Gameplay.Utils;
using UI.Core;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Gameplay.BottomPanel;
using UI.Gameplay.Validation;
using UI.Loading;
using UnityEngine;
using Zenject;

namespace Architecture.DI
{
    public class UIInstaller : MonoInstaller
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
            Container.BindInterfacesTo<ClusterFactory>().AsSingle();
            Container.BindInterfacesTo<WordContainerFactory>().AsSingle();
        }

        private void BindServices()
        {
            Container.BindInterfacesTo<UIClustersService>().AsSingle();
            Container.BindInterfacesTo<StubValidationService>().AsSingle();
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
            Container.BindPresenterWithView<UIBottomPanelPresenter, UIBottomPanelView>();
            Container.BindPresenterWithView<UIWordGridPresenter, UIWordGridView>();
            Container.BindPresenterWithView<UIGameplayPresenter, UIGameplayView>();
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