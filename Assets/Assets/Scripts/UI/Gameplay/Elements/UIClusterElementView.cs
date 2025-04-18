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
        
        private readonly List<UILetterView> _letters = new();
        
        public int LetterCount => _letters?.Count ?? 0;

        public event Action<UIClusterElementView> OnDragStarted;
        public event Action<Vector2> OnDragging;
        public event Action<PointerEventData> OnDragEnded;
        public event Action<UIClusterElementView> OnBeginDragFromContainer;
        
        private Transform OriginalParent { get; set; }

        private void Awake()
        {
            OriginalParent = transform.parent;
        }

        public void AddLetter(UILetterView letter) => 
            _letters.Add(letter);

        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalSiblingIndex = transform.GetSiblingIndex();
            if (transform.parent.TryGetComponent<UIWordContainerView>(out var container))
            {
                OnBeginDragFromContainer?.Invoke(this);
            }

            OnDragStarted?.Invoke(this);
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