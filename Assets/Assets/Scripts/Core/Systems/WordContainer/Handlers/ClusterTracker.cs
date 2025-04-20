using System.Linq;
using ModestTree;
using UI.Gameplay.Elements;

namespace Core.Systems.WordContainer
{
    public class ClusterTracker
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
                placedClusters[cluster] = Enumerable.Range(clusterPos, cluster.LetterCount).ToList();
            }

            if (bufferSlots.IsEmpty())
                return;
            
            var lowestSlot = bufferSlots.Aggregate((min, next) =>
                next.Index < min.Index ? next : min);
            
            _slotHandler.OccupySlots(lowestSlot.Index, bufferSlots.Count);
            bufferSlots.Clear();
        }
    }
}