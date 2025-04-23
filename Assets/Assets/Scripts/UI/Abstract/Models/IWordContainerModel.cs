using System;
using System.Collections.Generic;
using UI.Gameplay.Elements;
using UniRx;

namespace UI.Models
{
    public interface IWordContainerModel
    {
        public UILetterSlotView[] Slots { get; }
        public Dictionary<UIClusterElementView, List<int>> PlacedClusters { get; } 
        public Dictionary<UIClusterElementView, int> ClusterStartIndices { get; } 
        public List<int> ActivePlaceholderSlots { get; } 
        public List<UILetterSlotView> BufferSlots { get; } 
        
        public IReadOnlyReactiveProperty<bool> IsFullyFilled { get; }
        public IObservable<Unit> OnFullyFilled { get; }
        public IObservable<Unit> OnBecameIncomplete { get; }
        public void UpdateFilledSlotsStatus(bool flag);
    }
}