using UI.Gameplay.Elements;
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

        public UIWordContainerView[] CreateWordContainers(
            UIWordContainerView containerPrefab,
            Transform parentLayer,
            int letterPerWorld,
            int count
        );
    }
}