using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public class UIWordContainerView : MonoBehaviour, IClusterDropZone
    {
        [SerializeField] private UILetterSlotView _letterSlotPrefab;
        
        private UILetterSlotView[] _letterSlots;
        private readonly Dictionary<UIClusterElementView, List<int>> _placedClusters = new();
        private readonly Dictionary<UIClusterElementView, int> _clustersMap = new();
        private List<int> _currentPlaceholderSlots = new();
        private List<UILetterSlotView> _bufferSlots = new();

        public void Initialize(int wordLength)
        {
            _letterSlots = new UILetterSlotView[wordLength];
            for (var i = 0; i < wordLength; i++)
            {
                _letterSlots[i] = Instantiate(_letterSlotPrefab, transform);
                _letterSlots[i].Initialize(i);
            }
        }

        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex)
        {
            ClearPlaceholder();
            
            if (startIndex < 0 || startIndex + cluster.LetterCount > _letterSlots.Length)
                return;

            for (var i = 0; i < cluster.LetterCount; i++)
            {
                var slotIndex = startIndex + i;
                if (slotIndex >= 0 && slotIndex < _letterSlots.Length && !_letterSlots[slotIndex].IsOccupied)
                {
                    _letterSlots[slotIndex].SetAsPlaceholder(true);
                    _currentPlaceholderSlots.Add(slotIndex);
                }
            }
        }

        public void ClearPlaceholder()
        {
            for (var iterator = 0; iterator < _currentPlaceholderSlots.Count; iterator++)
            {
                var slotIndex = _currentPlaceholderSlots[iterator];
                if (slotIndex >= 0 && slotIndex < _letterSlots.Length)
                {
                    _letterSlots[slotIndex].SetAsPlaceholder(false);
                }
            }

            _currentPlaceholderSlots.Clear();
        }

        public bool TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            var targetSlotIndex = CalculateSlotIndexFromPosition(localPoint);
            var startIndex = targetSlotIndex - cluster.GrabbedLetterIndex;
            
            if (!IsValidDropPosition(startIndex, cluster.LetterCount))
                return false;
            
            _bufferSlots.Clear();
            
            if (_placedClusters.ContainsKey(cluster))
            {
                ReleaseSlotsForCluster(cluster);
            }
            
            OccupySlots(startIndex, cluster.LetterCount);
            
            cluster.transform.SetParent(transform);

            var newIndex = CalcSiblingIndex(startIndex);

            cluster.transform.SetSiblingIndex(newIndex);
            
            _placedClusters[cluster] = Enumerable.Range(startIndex, cluster.LetterCount).ToList();
            _clustersMap[cluster] = startIndex;
            
            ClearPlaceholder();
            
            return true;
        }

        private int CalcSiblingIndex(int startIndex)
        {
            var counterClusters = 0;

            foreach (var cluster in _placedClusters.Keys)
            {
                if (startIndex > cluster.transform.GetSiblingIndex())
                {
                    counterClusters++;
                }
            }

            startIndex += counterClusters;
            startIndex = Mathf.Clamp(startIndex, 0, startIndex);
            
            return startIndex;
        }

        private bool IsValidDropPosition(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex + length > _letterSlots.Length)
                return false;

            for (var i = 0; i < length; i++)
            {
                if (_letterSlots[startIndex + i].IsOccupied)
                    return false;
            }
            return true;
        }

        private void OccupySlots(int startIndex, int length)
        {
            for (var i = 0; i < length; i++)
            {
                _letterSlots[startIndex + i].SetOccupied(true);
            }
        }

        public int CalculateSlotIndexFromPosition(Vector2 localPosition)
        {
            if (_letterSlots == null || _letterSlots.Length == 0)
                return 0;

            var slotWidth = _letterSlots[0].GetComponent<RectTransform>().rect.width;
            var totalWidth = _letterSlots.Length * slotWidth;
            var normalizedPosition = (localPosition.x + totalWidth / 2f) / totalWidth;
            
            return Mathf.Clamp(Mathf.FloorToInt(normalizedPosition * _letterSlots.Length), 0, _letterSlots.Length - 1);
        }

        public void ReleaseSlotsForCluster(UIClusterElementView cluster)
        {
            if (_placedClusters.TryGetValue(cluster, out var slots))
            {
                foreach (var slotIndex in slots)
                {
                    if (slotIndex >= 0 && slotIndex < _letterSlots.Length)
                    {
                        _bufferSlots.Add(_letterSlots[slotIndex]);
                        _letterSlots[slotIndex].SetOccupied(false);
                    }
                }
                _placedClusters.Remove(cluster);
            }
        }

        public void ReturnClusterToPosition(UIClusterElementView cluster)
        {
            if (!_placedClusters.ContainsKey(cluster))
            {
                var clusterPos = _clustersMap[cluster];
                _placedClusters[cluster] = Enumerable.Range(clusterPos, cluster.LetterCount).ToList();
            }
            
            if(!_bufferSlots.IsEmpty())
            {
                var lowestSlot = _bufferSlots.Aggregate((min, next) =>
                    next.Index < min.Index ? next : min);
                OccupySlots(lowestSlot.Index, _bufferSlots.Count);
                _bufferSlots.Clear();
            }
        }

        public void ClearBuffers()
        {
            _bufferSlots.Clear();
        }
        
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null && 
                eventData.pointerDrag.TryGetComponent<UIClusterElementView>(out var cluster))
            {
                TryDrop(cluster, eventData);
            }
        }
    }
}