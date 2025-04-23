using UI.Gameplay.Elements;
using UI.Models;
using UI.Services;
using UnityEngine;

namespace UI.Factories
{
    public interface IUIWordContainerDependenciesFactory
    {
        public (IWordContainerModel, IWordSlotHandler, ISlotPlaceholderHelper, IContainerDropPlacementHelper, IClusterTracker)
            Create(
                UILetterSlotView[] letterSlots,
                Transform containerTransform,
                int containerIndex);
    }
}