using Core.Factories;
using Core.Services;
using Core.Services.WordContainers;
using Gameplay.Utils;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Settings;
using UI.Gameplay.Validation;
using UI.Loading;
using UI.Settings;
using UI.Victory;
using UnityEngine;
using Zenject;

namespace Architecture.DI
{
    public sealed class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _uiPrefab;

        public override void InstallBindings()
        {
            InstallFactories();

            BindServices();
            
            CreateUI();

            BindWindows();

            BindGameplayButtons();
            
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

        private void CreateUI()
        {
            Container.InstantiatePrefab(_uiPrefab);
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
            Container.BindPresenterWithView<UISettingsPanelPresenter, UISettingsPanelView>();
        }

        private void BindGameplayButtons()
        {
            Container.BindPresenterWithView<UIValidationButtonPresenter, UIValidationButtonView>();
            Container.BindPresenterWithView<UISettingsButtonPresenter, UISettingsButtonView>();
        }

        private void BindFlow()
        {
            Container.BindInterfacesAndSelfTo<UIFlowManager>().AsSingle();
        }
    }
}