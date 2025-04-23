using Cysharp.Threading.Tasks;
using UI.Abstract;
using UnityEngine;

namespace UI.Loaders
{
    public interface IUIWindowLoader
    {
        UniTask<TView> LoadWindowAsync<TView>(string address) where TView : Component, IUIView;
    }
}