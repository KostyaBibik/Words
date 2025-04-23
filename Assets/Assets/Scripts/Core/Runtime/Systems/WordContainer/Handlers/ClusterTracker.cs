using System.Collections.Generic;
using ModestTree;
using UI.Gameplay.Elements;
using UI.Services;

namespace Core.Systems.WordContainer
{
    public class ClusterTracker : IClusterTracker
    {
        private readonly WordContainerData _data;
        private readonly WordSlotHandler _slotHandler;

        public ClusterTracker(WordContainerData data, WordSlotHandler slotHandler)
        {
            _data = data;
            _slotHandler = slotHandler;
        }

        public void ReturnCluster(UIClusterElementView cluster)
        {
            var placedClusters = _data.PlacedClusters;
            var clusterStartIndices = _data.ClusterStartIndices;
            var bufferSlots = _data.BufferSlots;
            
            if (!placedClusters.ContainsKey(cluster))
            {
                var clusterPos = clusterStartIndices[cluster];
                var slots = new List<int>(cluster.Presenter.GetLettersCount());
                
                for (var iterator = 0; iterator < cluster.Presenter.GetLettersCount(); iterator++)
                {
                    slots.Add(clusterPos + iterator);
                }
                
                placedClusters[cluster] = slots;
            }

            if (bufferSlots.IsEmpty())
                return;
            
            var min = bufferSlots[0];
            for (var iterator = 1; iterator < bufferSlots.Count; iterator++)
            {
                if (bufferSlots[iterator].Index < min.Index)
                {
                    min = bufferSlots[iterator];
                }
            }
            
            _slotHandler.OccupySlots(min.Index, bufferSlots.Count);
            bufferSlots.Clear();
            _slotHandler.ReevaluateFullState();
        }
    }
}