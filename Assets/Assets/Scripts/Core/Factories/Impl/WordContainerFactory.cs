using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UnityEngine;
using UnityEngine.UI;

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

        public UIWordContainerPresenter[] CreateWordContainers(
            UIWordContainerView containerPrefab,
            Transform parentLayer,
            int letterPerWorld,
            int count
        )
        {
            var wordContainers = new UIWordContainerPresenter[count];
            for (var iterator = 0; iterator < count; iterator++)
            {
                var view = Object.Instantiate(containerPrefab, parentLayer);
                var presenter = new UIWordContainerPresenter(view, this, iterator);
                
                presenter.InitializeContainer(letterPerWorld);
                view.Initialize(presenter);
                
                wordContainers[iterator] = presenter;
            }

            var rectTransform = parentLayer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
            return wordContainers;
        }
    }
}