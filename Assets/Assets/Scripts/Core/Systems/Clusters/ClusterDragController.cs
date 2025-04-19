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
        private UIWordContainerView _lastHoveredContainer;

        public ClusterDragController(Transform dragLayer, Canvas canvas)
        {
            _dragLayer = dragLayer;
            _canvasRectTransform = canvas.GetComponent<RectTransform>();
        }

        public void HandleBeginDrag(UIClusterElementView cluster)
        {
            _currentCluster = cluster;
            _lastHoveredContainer = null;

            var rt = _currentCluster.GetComponent<RectTransform>();
            rt.SetParent(_dragLayer);
            rt.SetAsLastSibling();

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasRectTransform,
                Input.mousePosition,
                null,
                out var worldPoint);

            _offset = rt.position - worldPoint;

            if (_currentCluster.OriginalParent.TryGetComponent(out UIWordContainerView oldContainer))
            {
                oldContainer.ReleaseSlotsForCluster(_currentCluster);
            }
        }

        public void HandleDrag(Vector2 screenPosition)
        {
            if (_currentCluster == null)
                return;

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasRectTransform,
                screenPosition,
                null,
                out var worldPoint);

            _currentCluster.transform.position = worldPoint + _offset;

            var results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            }, results);

            var foundContainer = false;
            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<UIWordContainerView>(out var container))
                {
                    if (_lastHoveredContainer != container)
                    {
                        _lastHoveredContainer?.ClearPlaceholder();
                        _lastHoveredContainer = container;
                    }

                    RectTransformUtility.ScreenPointToLocalPointInRectangle(
                        (RectTransform)container.transform,
                        screenPosition,
                        null,
                        out var localPoint);

                    var targetSlotIndex = container.CalculateSlotIndexFromPosition(localPoint);
                    var startIndex = targetSlotIndex - _currentCluster.GrabbedLetterIndex;

                    container.ShowPlaceholder(_currentCluster, startIndex);
                    foundContainer = true;
                    break;
                }
            }

            if (!foundContainer)
            {
                _lastHoveredContainer?.ClearPlaceholder();
                _lastHoveredContainer = null;
            }
        }

        public void HandleEndDrag(PointerEventData eventData)
        {
            if (_currentCluster == null) return;

            var wasDropped = false;
            var results = new List<RaycastResult>();
            var oldContainer = _currentCluster.Container;
            UIWordContainerView targetContainer = null;
            
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent(out targetContainer))
                {
                    if (targetContainer.TryDrop(_currentCluster, eventData))
                    {
                        wasDropped = true;
                        _currentCluster.SetContainer(targetContainer);
                        break;
                    }
                }
            }

            if (!wasDropped)
            {
                _currentCluster.ReturnToOriginalPosition();
                if (_currentCluster.Container != null)
                {
                    _currentCluster.Container.ReturnClusterToPosition(_currentCluster);
                }
            }
            else
            {
                if (oldContainer != null && oldContainer != targetContainer)
                {
                    oldContainer.ClearBuffers();
                }
            }

            _lastHoveredContainer?.ClearPlaceholder();
            _lastHoveredContainer = null;
            _currentCluster = null;
        }
    }
}