using System;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Loaders;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace Core.Runtime
{
    public class AddressablesWindowLoader : IUIWindowLoader
    {
        private const string UI_ROOT_NAME = "UIRoot";
        
        private readonly DiContainer _container;
        private readonly Transform _uiRoot;

        public AddressablesWindowLoader(DiContainer container)
        {
            _container = container;
            _uiRoot = new GameObject(UI_ROOT_NAME).transform;
        }

        public async UniTask<TView> LoadWindow<TView>(string address) 
            where TView : Component, IUIView
        {
            var handle = Addressables.LoadAssetAsync<GameObject>(address);
            
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception($"Failed to load {address}");

            var prefab = handle.Result;
            var instance = _container.InstantiatePrefab(prefab, _uiRoot.transform).GetComponent<TView>();

            if (instance == null)
                throw new Exception($"Prefab at {address} doesn't contain component of type {typeof(TView)}");

            return instance;
        }
    }

}