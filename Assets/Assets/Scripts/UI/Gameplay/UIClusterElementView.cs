using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class UIClusterElementView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private TextMeshProUGUI _text;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;

        public event Action<PointerEventData> OnDragStarted;
        public event Action<PointerEventData> OnDragging;
        public event Action<PointerEventData> OnDragEnded;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void Setup(string text)
        {
            _text.text = text;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
            OnDragStarted?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            OnDragging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = true;
            OnDragEnded?.Invoke(eventData);
        }

        public void SetParentAndPosition(Transform parent, Vector3 position)
        {
            transform.SetParent(parent);
            _rectTransform.localPosition = position;
        }
    }
}