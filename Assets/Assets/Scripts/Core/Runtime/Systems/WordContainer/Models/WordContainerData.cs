using System;
using System.Collections.Generic;
using UI.Gameplay.Elements;
using UI.Models;
using UniRx;

namespace Core.Systems.WordContainer
{
    public class WordContainerData : IWordContainerModel
    {
        public UILetterSlotView[] Slots { get; private set; }
        public Dictionary<UIClusterElementView, List<int>> PlacedClusters { get; } = new();
        public Dictionary<UIClusterElementView, int> ClusterStartIndices { get; } = new();
        public List<int> ActivePlaceholderSlots { get; } = new();
        public List<UILetterSlotView> BufferSlots { get; } = new();
        
        private readonly ReactiveProperty<bool> _isFullyFilled = new(false);
        
        public IReadOnlyReactiveProperty<bool> IsFullyFilled => _isFullyFilled;
        public IObservable<Unit> OnFullyFilled => _isFullyFilled.Where(x => x).AsUnitObservable();
        public IObservable<Unit> OnBecameIncomplete => _isFullyFilled.Where(x => !x).AsUnitObservable();
        
        public WordContainerData(UILetterSlotView[] slots) => 
            Slots = slots;

        public void UpdateFilledSlotsStatus(bool flag) =>
            _isFullyFilled.Value = flag;
    }
}