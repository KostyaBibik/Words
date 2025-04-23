using UnityEngine;

namespace UI.Services
{
    public interface IContainerDropPlacementHelper : IDropPlacementHelper
    {
        public int CalculateSlotIndexFromPosition(Vector2 localPosition);
    }
}