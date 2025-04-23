using Core.Systems.WordContainer;
using UI.Factories;
using UI.Gameplay.Elements;
using UI.Models;
using UI.Services;
using UnityEngine;

namespace Core.Factories
{
    public class UIWordContainerDependenciesFactory : IUIWordContainerDependenciesFactory
    {
        public (IWordContainerModel, IWordSlotHandler, ISlotPlaceholderHelper, IContainerDropPlacementHelper, IClusterTracker) Create(
            UILetterSlotView[] letterSlots, 
            Transform containerTransform, 
            int containerIndex)
        {
            var model = new WordContainerData(letterSlots);
            var slotHandler = new WordSlotHandler(model);
            var placeholderHelper = new SlotPlaceholderHelper(model);
            var dropHelper = new ContainerDropPlacementHelper(model, slotHandler, placeholderHelper, containerTransform, containerIndex);
            var tracker = new ClusterTracker(model, slotHandler);
        
            return (model, slotHandler, placeholderHelper, dropHelper, tracker);
        }
    }
}