using Cysharp.Threading.Tasks;
using UI.Abstract;
using UnityEngine;

namespace UI.Loaders
{
    public interface IUIWindowLoader
    {
        public UniTask<TView> LoadWindow<TView>(string address) where TView : Component, IUIView;

        /// <summary>Loads and instantiates a prefab that isn't part of the window/presenter flow (no IUIView).</summary>
        public UniTask<TComponent> LoadComponent<TComponent>(string address) where TComponent : Component;
    }
}