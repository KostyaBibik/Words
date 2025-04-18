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

        private void Awake()
        {
            _placedClusters = new Dictionary<UIClusterElementView, List<int>>();
        }

        public void Initialize(int wordLength )
        {
            _letterSlots = new UILetterSlotView[wordLength];

            for (int i = 0; i < wordLength; i++)
            {
                var slotGO = Instantiate(_letterSlotPrefab, transform);
                var slot = slotGO.GetComponent<UILetterSlotView>();
                
                _letterSlots[i] = slot;
            }
        }

        public bool TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);

            var startIndex = CalculateSlotIndexFromPosition(localPoint);
            startIndex = Mathf.Clamp(startIndex, 0, _letterSlots.Length - cluster.LetterCount);

            if (!IsSpaceFree(startIndex, cluster.LetterCount))
            {
                return false;
            }
            
            for (var i = 0; i < cluster.LetterCount; i++)
            {
                var slotIndex = startIndex + i;
                _letterSlots[slotIndex].gameObject.SetActive(false); 
            }

            cluster.transform.SetParent(transform);
            cluster.transform.SetSiblingIndex(startIndex);  

            _placedClusters[cluster] = Enumerable.Range(startIndex, cluster.LetterCount).ToList();
            
            return true;
        }
        
        private int CalculateSlotIndexFromPosition(Vector2 localPosition)
        {
            var slotWidth = _letterSlots[0].GetComponent<RectTransform>().rect.width;
            var calculatedIndex = Mathf.Clamp(
                Mathf.FloorToInt(localPosition.x / slotWidth + _letterSlots.Length / 2f),
                0,
                _letterSlots.Length - 1);
            
            return calculatedIndex;
        }
        
        private bool IsSpaceFree(int startIndex, int length)
        {
            for (var i = 0; i < length; i++)
            {
                if (_letterSlots[startIndex + i].IsOccupied)
                    return false;
            }
            
            return true;
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
