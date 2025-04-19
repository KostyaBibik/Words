using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public class UIWordContainerView : MonoBehaviour, IClusterDropZone
    {
        [SerializeField] private UILetterSlotView _letterSlotPrefab;
        
        private UILetterSlotView[] _letterSlots;
        private Dictionary<UIClusterElementView, List<int>> _placedClusters;
        private List<int> _currentPlaceholderSlots = new();

        private void Awake()
        {
            _placedClusters = new Dictionary<UIClusterElementView, List<int>>();
        }

        public void Initialize(int wordLength)
        {
            _letterSlots = new UILetterSlotView[wordLength];
            for (int i = 0; i < wordLength; i++)
            {
                _letterSlots[i] = Instantiate(_letterSlotPrefab, transform);
                _letterSlots[i].name = $"LetterSlot_{i}";
            }
        }

        public void ShowPlaceholder(UIClusterElementView cluster, int startIndex)
        {
            ClearPlaceholder();
            
            if (startIndex < 0 || startIndex + cluster.LetterCount > _letterSlots.Length)
                return;

            for (int i = 0; i < cluster.LetterCount; i++)
            {
                int slotIndex = startIndex + i;
                if (slotIndex >= 0 && slotIndex < _letterSlots.Length && !_letterSlots[slotIndex].IsOccupied)
                {
                    _letterSlots[slotIndex].SetAsPlaceholder(true);
                    _currentPlaceholderSlots.Add(slotIndex);
                }
            }
        }

        public void ClearPlaceholder()
        {
            foreach (var index in _currentPlaceholderSlots)
            {
                if (index >= 0 && index < _letterSlots.Length)
                {
                    _letterSlots[index].SetAsPlaceholder(false);
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

            int targetSlotIndex = CalculateSlotIndexFromPosition(localPoint);
            int startIndex = targetSlotIndex - cluster.GrabbedLetterIndex;

            if (!IsValidDropPosition(startIndex, cluster.LetterCount))
                return false;

            if (_placedClusters.ContainsKey(cluster))
            {
                ReleaseSlotsForCluster(cluster);
            }

            OccupySlots(startIndex, cluster.LetterCount);
            
            cluster.transform.SetParent(transform);
            cluster.transform.SetSiblingIndex(startIndex);
            
            _placedClusters[cluster] = Enumerable.Range(startIndex, cluster.LetterCount).ToList();
            ClearPlaceholder();
            
            return true;
        }

        private bool IsValidDropPosition(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex + length > _letterSlots.Length)
                return false;

            for (int i = 0; i < length; i++)
            {
                if (_letterSlots[startIndex + i].IsOccupied)
                    return false;
            }
            return true;
        }

        private void OccupySlots(int startIndex, int length)
        {
            for (int i = 0; i < length; i++)
            {
                _letterSlots[startIndex + i].SetOccupied(true);
            }
        }

        public int CalculateSlotIndexFromPosition(Vector2 localPosition)
        {
            if (_letterSlots == null || _letterSlots.Length == 0)
                return 0;

            float slotWidth = _letterSlots[0].GetComponent<RectTransform>().rect.width;
            float totalWidth = _letterSlots.Length * slotWidth;
            float normalizedPosition = (localPosition.x + totalWidth / 2f) / totalWidth;
            
            return Mathf.Clamp(Mathf.FloorToInt(normalizedPosition * _letterSlots.Length), 0, _letterSlots.Length - 1);
        }

        public void ReleaseSlotsForCluster(UIClusterElementView cluster)
        {
            if (_placedClusters.TryGetValue(cluster, out var slots))
            {
                foreach (int slotIndex in slots)
                {
                    if (slotIndex >= 0 && slotIndex < _letterSlots.Length)
                    {
                        _letterSlots[slotIndex].SetOccupied(false);
                    }
                }
                _placedClusters.Remove(cluster);
            }
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