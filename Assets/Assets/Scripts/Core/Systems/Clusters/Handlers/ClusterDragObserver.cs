using Core.Systems.Placeholder;
using UI.Gameplay.Elements;
using UnityEngine;
using UniRx;

namespace UI.Gameplay
{
    public sealed class ClusterDragObserver
    {
        private readonly ClusterDragCoordinator _dragCoordinator;
        private readonly ClusterPlaceholderHandler _placeholderHandler;

        public ClusterDragObserver(ClusterDragCoordinator dragCoordinator, ClusterPlaceholderHandler placeholderHandler)
        {
            _dragCoordinator = dragCoordinator;
            _placeholderHandler = placeholderHandler;
        }

        public void Observe(UIClusterElementView cluster, MonoBehaviour lifeScope)
        {
            cluster.OnDragStarted.Subscribe(eventData =>
            {
                var index = cluster.transform.GetSiblingIndex();
                if (cluster.Presenter.GetContainer() == null)
                {
                    _placeholderHandler.ActivatePlaceholder(cluster, index);
                }
                _dragCoordinator.HandleBeginDrag(cluster);
            }).AddTo(lifeScope);

            cluster.OnDragging.Subscribe(position =>
            {
                _dragCoordinator.HandleDrag(position);
            }).AddTo(lifeScope);

            cluster.OnDragEnded.Subscribe(eventData =>
            {
                _placeholderHandler.DeactivatePlaceholder();
                _dragCoordinator.HandleEndDrag(eventData);
            }).AddTo(lifeScope);
        }
    }
}