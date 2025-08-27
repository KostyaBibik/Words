using Cysharp.Threading.Tasks;
using UI.Abstract;
using UnityEngine;

namespace UI.Loaders
{
    public interface IUIWindowLoader
    {
        public UniTask<TView> LoadWindow<TView>(string address) where TView : Component, IUIView;
    }
}