using UI.Gameplay.Elements;
using UnityEngine;

namespace Core.Factories
{
    public class WordContainerFactory : IWordContainerFactory
    {
        public UILetterSlotView[] CreateLetterSlot(
            UILetterSlotView letterSlotPrefab, 
            Transform parentLayer, 
            int count
        )
        {
            var letterSlots = new UILetterSlotView[count];
            for (var i = 0; i < count; i++)
            {
                letterSlots[i] = Object.Instantiate(letterSlotPrefab, parentLayer);
                letterSlots[i].Initialize(i);
            }

            return letterSlots;
        }

        public UIWordContainerView[] CreateWordContainers(
            UIWordContainerView containerPrefab,
            Transform parentLayer,
            int letterPerWorld,
            int count
        )
        {
            var wordContainers = new UIWordContainerView[count];
            for (var i = 0; i < count; i++)
            {
                wordContainers[i] = Object.Instantiate(containerPrefab, parentLayer);
                wordContainers[i].Initialize(letterPerWorld);
            }

            return wordContainers;
        }
    }
}