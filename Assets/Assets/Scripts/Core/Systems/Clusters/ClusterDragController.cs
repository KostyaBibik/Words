using System.Collections.Generic;
using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public class ClusterDragController
    {
        private Transform _dragLayer;
        private UIClusterElementView _currentCluster;
        private Vector3 _offset; 
        private RectTransform _canvasRectTransform;
        
        public ClusterDragController(Transform dragLayer, Canvas canvas)
        {
            _dragLayer = dragLayer;
            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        public void HandleBeginDrag(UIClusterElementView cluster)
        {
            _currentCluster = cluster;
            var rt = _currentCluster.GetComponent<RectTransform>();
        
            rt.SetParent(_dragLayer);
            rt.SetAsLastSibling();
        
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasRectTransform,
                Input.mousePosition,
                null,
                out var worldPoint);
        
            _offset = rt.position - worldPoint;
        }

        public void HandleDrag(Vector2 screenPosition)
        {
            if (_currentCluster == null) return;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasRectTransform,
                screenPosition,
                null,
                out var worldPoint);
        
            _currentCluster.transform.position = worldPoint + _offset;
        }

        public void HandleEndDrag(PointerEventData eventData)
        {
            if (_currentCluster == null) return;

            bool wasDropped = false;
    
            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<UIWordContainerView>(out var dropZone))
                {
                    if (dropZone.TryDrop(_currentCluster, eventData))
                    {
                        wasDropped = true;
                        break;
                    }
                }
            }

            if (!wasDropped)
            {
                _currentCluster.ReturnToOriginalPosition();
            }

            _currentCluster = null;
        }
    }
}
