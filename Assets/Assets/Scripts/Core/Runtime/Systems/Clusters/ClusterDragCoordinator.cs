using UI.Gameplay.Elements;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public sealed class ClusterDragCoordinator
    {
        private readonly DragStartHandler _dragStartHandler;
        private readonly DragMoveHandler _dragMoveHandler;
        private readonly DragEndHandler _dragEndHandler;

        private UIClusterElementView _currentCluster;
        private UIWordContainerView _lastHoveredContainer;
        private Vector3 _dragOffset;

        public ClusterDragCoordinator(Transform dragLayer, Canvas canvas)
        {
            var canvasRectTransform = canvas.GetComponent<RectTransform>();

            _dragStartHandler = new DragStartHandler(canvasRectTransform, dragLayer);
            _dragMoveHandler = new DragMoveHandler(canvasRectTransform);
            _dragEndHandler = new DragEndHandler();
        }

        public void HandleBeginDrag(UIClusterElementView cluster)
        {
            _currentCluster = cluster;
            _lastHoveredContainer = null;

            _dragOffset = _dragStartHandler.BeginDrag(cluster);

            if (cluster.Presenter.GetOriginalParent().TryGetComponent(out UIWordContainerView oldContainer))
            {
                oldContainer.Presenter.ReleaseSlotsForCluster(cluster);
            }
        }

        public void HandleDrag(Vector2 screenPosition)
        {
            if (_currentCluster == null)
                return;

            _dragMoveHandler.UpdateClusterPosition(_currentCluster, screenPosition, _dragOffset);

            var (container, found) = _dragMoveHandler.HandleRaycastAndPlaceholder(
                _currentCluster, screenPosition, _lastHoveredContainer);

            _lastHoveredContainer = container;

            if (!found)
            {
                _lastHoveredContainer?.Presenter.ClearPlaceholder();
                _lastHoveredContainer = null;
            }
        }

        public void HandleEndDrag(PointerEventData eventData)
        {
            if (_currentCluster == null)
                return;

            _dragEndHandler.CompleteDrag(_currentCluster, eventData, _lastHoveredContainer);

            _lastHoveredContainer = null;
            _currentCluster = null;
        }
    }
}
