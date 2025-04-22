using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UnityEngine;

namespace Core.Factories
{
    public interface IWordContainerFactory
    {
        public UILetterSlotView[] CreateLetterSlots(
            UILetterSlotView letterSlotPrefab,
            Transform parentLayer,
            int count
        );

        public UIWordContainerPresenter[] CreateWordContainers(
            UIWordContainerView containerPrefab,
            Transform parentLayer,
            int letterPerWorld,
            int count
        );
    }
}