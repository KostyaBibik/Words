using UnityEngine;

namespace Core.Systems.WordContainer
{
    public interface IContainerDropPlacementHelper : IDropPlacementHelper
    {
        public int CalculateSlotIndexFromPosition(Vector2 localPosition);
    }
}