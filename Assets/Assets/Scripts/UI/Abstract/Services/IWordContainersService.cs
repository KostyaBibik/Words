using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UnityEngine;

namespace UI.Services
{
    public interface IWordContainersService
    {
        public void UpdateClusters(UIWordContainerView prefab, Transform parent, int wordCount, int lettersPerWord);
        public void Clear();
        public UIWordContainerPresenter[] ContainerPresenters { get; }
    }
}