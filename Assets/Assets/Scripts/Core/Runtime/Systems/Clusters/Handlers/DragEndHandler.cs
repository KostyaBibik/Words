using UI.Gameplay.Elements;
using UI.Gameplay.Utils;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public sealed class DragEndHandler
    {
        public async void CompleteDrag(UIClusterElementView cluster, PointerEventData eventData, UIWordContainerView lastHovered)
        {
            var results = UIRaycastHelper.RaycastWithEventData(eventData);
            var oldContainer = cluster.Presenter.GetContainer();
            var wasDropped = false;

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<IClusterDropZone>(out var dropZone))
                {
                    var dropped = await dropZone.TryDrop(cluster, eventData);
                    if (!dropped)
                        continue;

                    if (dropZone is UIWordContainerView wordContainer)
                    {
                        cluster.Presenter.SetContainer(wordContainer);
                    }

                    wasDropped = true;
                    break;
                }
            }

            if (!wasDropped)
            {
                ClusterReverter.ReturnToOriginal(cluster);
            }
            else if (oldContainer != null)
            {
                oldContainer.Presenter.ClearBuffers();
            }

            lastHovered?.Presenter.ClearPlaceholder();
        }
    }
}