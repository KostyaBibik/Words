using System.Linq;
using UI.Gameplay.Elements;
using UI.Services;

namespace Core.Systems.WordContainer
{
    public class WordSlotHandler : IWordSlotHandler
    {
        private readonly WordContainerData _data;

        public WordSlotHandler(WordContainerData data) =>
            _data = data;

        public bool IsValidDropPosition(int startIndex, int length)
        {
            var letterSlots = _data.Slots;
            
            if (startIndex < 0 || startIndex + length > letterSlots.Length)
                return false;

            for (var i = 0; i < length; i++)
            {
                if (letterSlots[startIndex + i].IsOccupied)
                    return false;
            }
            return true;
        }

        public void OccupySlots(int startIndex, int length)
        {
            for (var iterator = 0; iterator < length; iterator++)
            {
                _data.Slots[startIndex + iterator].SetOccupied(true);
            }
        }

        public void ReleaseSlots(UIClusterElementView cluster)
        {
            var placedClusters = _data.PlacedClusters;
            var bufferSlots = _data.BufferSlots;
            var letterSlots = _data.Slots;
            
            if (placedClusters.TryGetValue(cluster, out var slots))
            {
                for (var iterator = 0; iterator < slots.Count; iterator++)
                {
                    var slotIndex = slots[iterator];
                    
                    if (slotIndex >= 0 && slotIndex < letterSlots.Length)
                    {
                        bufferSlots.Add(letterSlots[slotIndex]);
                        letterSlots[slotIndex].SetOccupied(false);
                    }
                }

                placedClusters.Remove(cluster);
            }

            ReevaluateFullState();
        }
        
        public void ReevaluateFullState()
        {
            var allFilled = _data.Slots.All(s => s.IsOccupied);
            _data.UpdateFilledSlotsStatus(allFilled);
        }
    }
}