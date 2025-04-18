using UI.Gameplay;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Systems.Clusters
{
    public class ClusterDragController
    {
        private readonly Canvas _rootCanvas;
        private readonly Transform _dragLayer;
        private UIClusterElementView _draggedItem;
        private Vector3 _originalPosition;
        private Transform _originalParent;
        private int _originalSiblingIndex;
        
        public ClusterDragController(Canvas rootCanvas, Transform dragLayer)
        {
            _rootCanvas = rootCanvas;
            _dragLayer = dragLayer;
        }

        public void HandleBeginDrag(UIClusterElementView item)
        {
            _draggedItem = item;
            _originalParent = item.transform.parent;
            _originalSiblingIndex = item.transform.GetSiblingIndex();
        
            item.transform.SetParent(_dragLayer); 
        }

        public void HandleDrag(PointerEventData eventData)
        {
            if (_draggedItem == null) return;
        
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _rootCanvas.GetComponent<RectTransform>(),
                eventData.position,
                eventData.pressEventCamera,
                out var worldPoint);
            
            _draggedItem.transform.position = worldPoint;
        }

        public void HandleEndDrag(bool wasDropped)
        {
            if (_draggedItem == null || wasDropped) return;
        
            _draggedItem.transform.SetParent(_originalParent);
            _draggedItem.transform.SetSiblingIndex(_originalSiblingIndex);
        }
    }
}