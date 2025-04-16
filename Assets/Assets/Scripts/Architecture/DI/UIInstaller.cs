using Assets.Scripts.UI.Core;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Architecture.DI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _uiManagerPrefab;

        public override void InstallBindings()
        {
            var uiManager = Container.InstantiatePrefab(_uiManagerPrefab);
            Container.Bind<UIManager>().FromComponentOn(uiManager).AsSingle();
        }

        private void BindPresenter<TView, TPresenter>() 
            where TView : MonoBehaviour, IUIView
        {
            Container.Bind<TView>()
                .FromComponentInChildren()
                .WhenInjectedInto<TPresenter>();
            
            Container.Bind<TPresenter>().AsSingle();
        }
    }
}