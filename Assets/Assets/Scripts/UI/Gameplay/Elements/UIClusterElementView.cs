using System;
using System.Collections.Generic;
using UniRx;
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

        private readonly Subject<UIClusterElementView> _onDragStarted = new();
        private readonly Subject<Vector2> _onDragging = new();
        private readonly Subject<PointerEventData> _onDragEnded = new();

        public IObservable<UIClusterElementView> OnDragStarted => _onDragStarted.AsObservable();
        public IObservable<Vector2> OnDragging => _onDragging.AsObservable();
        public IObservable<PointerEventData> OnDragEnded => _onDragEnded.AsObservable();

        public Transform OriginalParent { get; private set; }
        public UIWordContainerView Container { get; private set; }

        private void Awake()
        {
            OriginalParent = transform.parent;
        }

        public void AddLetter(UILetterView letter) =>
            _letters.Add(letter);

        public void SetContainer(UIWordContainerView container) =>
            Container = container;

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

            _onDragStarted.OnNext(this); 
        }

        private int CalculateGrabbedLetterIndex(PointerEventData eventData)
        {
            var rect = GetComponent<RectTransform>().rect;
            var localX = _dragOffset.x + rect.width / 2f;
            var letterWidth = rect.width / LetterCount;

            return Mathf.Clamp(Mathf.FloorToInt(localX / letterWidth), 0, LetterCount - 1);
        }

        public void OnDrag(PointerEventData eventData)
        {
            _onDragging.OnNext(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _onDragEnded.OnNext(eventData);
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
