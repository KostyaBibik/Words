using System.Collections.Generic;
using UI.Gameplay.Elements;

namespace Core.Systems.WordContainer
{
    public class WordContainerData
    {
        public UILetterSlotView[] Slots { get; private set; }
        public Dictionary<UIClusterElementView, List<int>> PlacedClusters { get; } = new();
        public Dictionary<UIClusterElementView, int> ClusterStartIndices { get; } = new();
        public List<int> ActivePlaceholderSlots { get; } = new();
        public List<UILetterSlotView> BufferSlots { get; } = new();

        public WordContainerData(UILetterSlotView[] slots) => 
            Slots = slots;
    }
}