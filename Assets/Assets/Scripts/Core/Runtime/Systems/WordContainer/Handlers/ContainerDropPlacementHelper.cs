using System.Collections.Generic;
using UI.Gameplay.Elements;
using UI.Services;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Systems.WordContainer
{
    public class ContainerDropPlacementHelper : IContainerDropPlacementHelper
    {
        private readonly WordContainerData _data;
        private readonly WordSlotHandler _slotHandler;
        private readonly SlotPlaceholderHelper _placeholderHelper;
        private readonly Transform _viewTransform;
        private readonly int _wordGroupIndex;

        public ContainerDropPlacementHelper(
            WordContainerData data, 
            WordSlotHandler slotHandler, 
            SlotPlaceholderHelper placeholderHelper,
            Transform viewTransform,
            int wordGroupIndex
        )
        {
            _data = data;
            _slotHandler = slotHandler;
            _placeholderHelper = placeholderHelper;
            _viewTransform = viewTransform;
            _wordGroupIndex = wordGroupIndex;
        }

        public bool TryDropCluster(
            UIClusterElementView cluster,
            PointerEventData eventData,
            Transform parentLayer
        )
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_viewTransform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            var targetSlotIndex = CalculateSlotIndexFromPosition(localPoint);
            var startIndex = targetSlotIndex - cluster.Presenter.GetGrabbedLetterIndex();
            
            if (!_slotHandler.IsValidDropPosition(startIndex, cluster.Presenter.GetLettersCount()))
                return false;

            var bufferSlots = _data.BufferSlots;
            var placedClusters = _data.PlacedClusters;
            var clustersStartIndices = _data.ClusterStartIndices;
            
            bufferSlots.Clear();
            
            if (placedClusters.ContainsKey(cluster))
            {
                ReleaseSlotsForCluster(cluster);
            }
            
            _slotHandler.OccupySlots(startIndex, cluster.Presenter.GetLettersCount());
            
            cluster.transform.SetParent(_viewTransform);

            var newIndex = CalculateSiblingIndex(startIndex);

            cluster.transform.SetSiblingIndex(newIndex);

            var lettersCount = cluster.Presenter.GetLettersCount();
            var slots = new List<int>(lettersCount);
            
            for (var iterator = 0; iterator < lettersCount; iterator++)
            {
                slots.Add(startIndex + iterator);
            }
            
            placedClusters[cluster] = slots;
            clustersStartIndices[cluster] = startIndex;
            
            _placeholderHelper.ClearPlaceholder();

            UpdateClustersOrderInWord();
            UpdateClusterContainerIndex(cluster);
            _slotHandler.ReevaluateFullState();
            
            return true;
        }

        public int CalculateSlotIndexFromPosition(Vector2 localPosition)
        {
            if (_data.Slots == null || _data.Slots.Length == 0)
                return 0;

            var slotWidth = _data.Slots[0].GetComponent<RectTransform>().rect.width;
            var totalWidth = _data.Slots.Length * slotWidth;
            var normalizedPosition = (localPosition.x + totalWidth / 2f) / totalWidth;
            
            return Mathf.Clamp(Mathf.FloorToInt(normalizedPosition * _data.Slots.Length), 0, _data.Slots.Length - 1);
        }

        private void ReleaseSlotsForCluster(UIClusterElementView cluster)
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
        }
        
        private int CalculateSiblingIndex(int startIndex)
        {
            var counter = 0;
            
            foreach (var cluster in _data.PlacedClusters.Keys)
            {
                if (startIndex > cluster.transform.GetSiblingIndex())
                {
                    counter++;
                }
            }
            
            return Mathf.Clamp(startIndex + counter, 0, startIndex + counter);
        }

        private void UpdateClustersOrderInWord()
        {
            var placedClusters = _data.PlacedClusters;

            var clusters = new List<UIClusterElementView>();
            foreach (var cluster in placedClusters.Keys)
            {
                clusters.Add(cluster);
            }

            for (var i = 0; i < clusters.Count - 1; i++)
            {
                for (var j = i + 1; j < clusters.Count; j++)
                {
                    if (clusters[i].transform.GetSiblingIndex() > clusters[j].transform.GetSiblingIndex())
                    {
                        (clusters[i], clusters[j]) = (clusters[j], clusters[i]);
                    }
                }
            }

            for (var i = 0; i < clusters.Count; i++)
            {
                clusters[i].Presenter.SetOrderInWord(i);
            }
        }

        private void UpdateClusterContainerIndex(UIClusterElementView clusterElementView) =>
            clusterElementView.Presenter.SetWordGroupIndex(_wordGroupIndex);
    }
}