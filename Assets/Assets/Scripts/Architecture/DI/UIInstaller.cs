using Extensions;
using UI.Core;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Loading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Architecture.DI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _uiManagerPrefab;

        public override void InstallBindings()
        {
            CreateAndBindUIManager();
            
            BindWindows();
            
            BindFlow();
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
            Container.BindPresenterWithView<UIGameplayPresenter, UIGameplayView>();
        }

        private void BindFlow()
        {
            Container.BindInterfacesAndSelfTo<UIFlowManager>().AsSingle();
        }
    }
}