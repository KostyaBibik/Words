using System.Collections.Generic;
using DataBase.Models;
using UI.Factories;
using UI.Gameplay.Elements;
using UI.Gameplay.WordContainers;
using UI.Victory.Grid;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Factories
{
    public sealed class UIWordContainerFactory : IUIWordContainerFactory
    {
        [Inject] private IUIWordContainerDependenciesFactory _dependenciesFactory;
        
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

            var rectTransform = parentLayer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
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
                var presenter = new UIWordContainerPresenter(view, this, _dependenciesFactory, iterator);
                
                presenter.InitializeContainer(letterPerWorld);
                view.Initialize(presenter);
                
                wordContainers[iterator] = presenter;
            }

            var rectTransform = parentLayer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
            return wordContainers;
        }

        public UIFinallyWordPresenter[] CreateFinallyWords(
            UIFinallyWordView elementPrefab,
            Transform parentLayer,
            IReadOnlyList<ValidatedWordData> data
        )
        {
            var presenters = new UIFinallyWordPresenter[data.Count];
            for (var iterator = 0; iterator < data.Count; iterator++)
            {
                var view = Object.Instantiate(elementPrefab, parentLayer);
                var presenter = new UIFinallyWordPresenter(view);

                var elementText = $"{data[iterator].filledOrder + 1} : {data[iterator].text}";
                presenter.UpdateData(elementText);
                
                presenters[iterator] = presenter;
            }

            var rectTransform = parentLayer.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            
            return presenters;
        }
    }
}