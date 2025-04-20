using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UnityEngine;

namespace Core.Factories
{
    public class WordContainerFactory : IWordContainerFactory
    {
        public UILetterSlotView[] CreateLetterSlots(
            UILetterSlotView letterSlotPrefab, 
            Transform parentLayer, 
            int count
        )
        {
            var letterSlots = new UILetterSlotView[count];
            for (var iterator = 0; iterator < count; iterator++)
            {
                letterSlots[iterator] = Object.Instantiate(letterSlotPrefab, parentLayer);
                letterSlots[iterator].Initialize(iterator);
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
            for (var iterator = 0; iterator < count; iterator++)
            {
                var view = Object.Instantiate(containerPrefab, parentLayer);
                var presenter = new UIWordContainerPresenter(view, this);
                
                presenter.InitializeContainer(letterPerWorld);
                view.Initialize(presenter);
                
                wordContainers[iterator] = view;
            }

            return wordContainers;
        }
    }
}