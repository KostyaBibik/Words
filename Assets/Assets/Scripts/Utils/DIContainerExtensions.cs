using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Loaders;
using UnityEngine;
using Zenject;

namespace Gameplay.Utils
{
    public static class DiContainerExtensions
    {
        public static void BindPresenterWithView<TPresenter, TView>(this DiContainer container)
            where TPresenter : UIPresenter<TView>
            where TView : IUIView
        {
            container.BindInterfacesAndSelfTo<TView>()
                .FromComponentInHierarchy()
                .AsSingle();

            container.BindInterfacesAndSelfTo<TPresenter>()
                .AsSingle()
                .NonLazy();
        }
        
        public static async UniTask<TPresenter> BindPresenterWithViewAsync<TPresenter, TView>(this DiContainer container)
            where TPresenter : UIPresenter<TView>
            where TView : Component, IUIView
        {
            container.BindInterfacesAndSelfTo<TView>()
                .FromComponentInHierarchy()
                .AsSingle();

            var view = container.Resolve<TView>();

            var presenter = container.Instantiate<TPresenter>(new object[] { view });

            if (presenter is IInitializable initializable)
                initializable.Initialize();

            container.BindInterfacesAndSelfTo<TPresenter>().FromInstance(presenter).AsSingle();

            return presenter;
        }
        
        public static async UniTask BindPresenterWithViewFromAddressableAsync<TPresenter, TView>(
            this DiContainer container,
            IUIWindowLoader loader,
            string address)
            where TPresenter : UIPresenter<TView>
            where TView : Component, IUIView
        {
            var view = await loader.LoadWindow<TView>(address);
            
            container.BindInterfacesAndSelfTo<TView>().FromInstance(view).AsSingle();
            
            var presenter = container.Instantiate<TPresenter>(new object[] { view });
            
            if (presenter is IInitializable initializable)
                initializable.Initialize();
            
            container.BindInterfacesAndSelfTo<TPresenter>().FromInstance(presenter).AsSingle();
        }
        
        public static async UniTask<TPresenter> BindPresenterWithViewFromAddressableAsync<TPresenter, TView>(
            this DiContainer container,
            string address,
            IUIWindowLoader loader)
            where TPresenter : UIPresenter<TView>
            where TView : Component, IUIView
        {
            var view = await loader.LoadWindow<TView>(address);

            container.BindInterfacesAndSelfTo<TView>().FromInstance(view).AsSingle();

            var presenter = container.Instantiate<TPresenter>(new object[] { view });
            if (presenter is IInitializable initializable)
                initializable.Initialize();
        
            container.BindInterfacesAndSelfTo<TPresenter>().FromInstance(presenter).AsSingle();

            return presenter;
        }
    }
}