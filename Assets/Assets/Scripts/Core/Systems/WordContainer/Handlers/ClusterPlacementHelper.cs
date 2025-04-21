using System.Collections.Generic;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Systems.WordContainer
{
    public class ClusterPlacementHelper
    {
        private readonly WordContainerData _data;
        private readonly WordSlotHandler _slotHandler;
        private readonly SlotPlaceholderHelper _placeholderHelper;
        private readonly Transform _viewTransform;

        public ClusterPlacementHelper(
            WordContainerData data, 
            WordSlotHandler slotHandler, 
            SlotPlaceholderHelper placeholderHelper,
            Transform viewTransform
        )
        {
            _data = data;
            _slotHandler = slotHandler;
            _placeholderHelper = placeholderHelper;
            _viewTransform = viewTransform;
        }

        public bool TryDropCluster(
            UIClusterElementView cluster, 
            PointerEventData eventData
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
            
            var slots = new List<int>(cluster.Presenter.GetLettersCount());
            for (var iterator = 0; iterator < cluster.Presenter.GetLettersCount(); iterator++)
            {
                slots.Add(startIndex + iterator);
            }
            
            placedClusters[cluster] = slots;
            clustersStartIndices[cluster] = startIndex;
            
            _placeholderHelper.ClearPlaceholder();
            
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
    }
}