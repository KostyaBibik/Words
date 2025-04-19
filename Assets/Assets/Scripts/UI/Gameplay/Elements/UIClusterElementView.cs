using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay.Elements
{
    public class UIClusterElementView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector3 _originalPosition;
        private Transform _originalParent;
        private int _originalSiblingIndex;
        private Vector2 _dragOffset;
        
        private readonly List<UILetterView> _letters = new();
        public int GrabbedLetterIndex { get; private set; }
        public int LetterCount => _letters?.Count ?? 0;

        public event Action<UIClusterElementView> OnDragStarted;
        public event Action<Vector2> OnDragging;
        public event Action<PointerEventData> OnDragEnded;
        public event Action<UIClusterElementView> OnBeginDragFromContainer;
        
        public Transform OriginalParent { get; private set; }

        private void Awake()
        {
            OriginalParent = transform.parent;
        }

        public void AddLetter(UILetterView letter) => 
            _letters.Add(letter);

        public Vector2 GetDragOffset(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out var localPoint);
    
            return localPoint;
        }
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            OriginalParent = transform.parent;
            _originalSiblingIndex = transform.GetSiblingIndex();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)transform,
                eventData.position,
                eventData.pressEventCamera,
                out _dragOffset);

            GrabbedLetterIndex = CalculateGrabbedLetterIndex(eventData);
            
            if (transform.parent.TryGetComponent<UIWordContainerView>(out var container))
            {
                OnBeginDragFromContainer?.Invoke(this);
            }

            OnDragStarted?.Invoke(this);
        }
        
        private int CalculateGrabbedLetterIndex(PointerEventData eventData)
        {
            var rect = GetComponent<RectTransform>().rect;
            float localX = _dragOffset.x + rect.width / 2f;
            float letterWidth = rect.width / LetterCount;

            return Mathf.Clamp(Mathf.FloorToInt(localX / letterWidth), 0, LetterCount - 1);
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            OnDragging?.Invoke(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragEnded?.Invoke(eventData);
        }

        public void ReturnToOriginalPosition()
        {
            transform.SetParent(OriginalParent);
            transform.localPosition = Vector3.zero;
            transform.SetSiblingIndex(_originalSiblingIndex);
            gameObject.SetActive(true); 
        }
    }
}