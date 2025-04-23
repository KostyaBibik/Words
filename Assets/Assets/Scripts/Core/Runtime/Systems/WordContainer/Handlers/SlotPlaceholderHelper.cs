using UI.Gameplay.Elements;
using UI.Services;

namespace Core.Systems.WordContainer
{
    public class SlotPlaceholderHelper : ISlotPlaceholderHelper
    {
        private readonly WordContainerData _data;

        public SlotPlaceholderHelper(WordContainerData data) 
            => _data = data;

        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex)
        {
            ClearPlaceholder();
            
            if (startIndex < 0 || startIndex + cluster.Presenter.GetLettersCount() > _data.Slots.Length)
                return;

            for (var i = 0; i < cluster.Presenter.GetLettersCount(); i++)
            {
                var slotIndex = startIndex + i;
                if (slotIndex >= 0 && slotIndex < _data.Slots.Length && !_data.Slots[slotIndex].IsOccupied)
                {
                    _data.Slots[slotIndex].SetAsPlaceholder(true);
                    _data.ActivePlaceholderSlots.Add(slotIndex);
                }
            }
        }

        public void ClearPlaceholder()
        {
            var activePlaceholders = _data.ActivePlaceholderSlots;
            var letterSlots = _data.Slots;
            
            for (var iterator = 0; iterator < activePlaceholders.Count; iterator++)
            {
                var slotIndex = activePlaceholders[iterator];
                if (slotIndex >= 0 && slotIndex < letterSlots.Length)
                {
                    letterSlots[slotIndex].SetAsPlaceholder(false);
                }
            }

            activePlaceholders.Clear();
        }
    }
}