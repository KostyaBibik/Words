using UI.Gameplay.Elements;
using UI.Gameplay.Utils;
using UnityEngine.EventSystems;

namespace UI.Gameplay
{
    public class DragEndHandler
    {
        public void CompleteDrag(UIClusterElementView cluster, PointerEventData eventData, UIWordContainerView lastHovered)
        {
            var results = UIRaycastHelper.RaycastWithEventData(eventData);
            var oldContainer = cluster.Container;
            var wasDropped = false;
            UIWordContainerView targetContainer = null;

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent(out targetContainer))
                {
                    if (targetContainer.TryDrop(cluster, eventData))
                    {
                        cluster.SetContainer(targetContainer);
                        wasDropped = true;
                        break;
                    }
                }
            }

            if (!wasDropped)
            {
                ClusterReverter.ReturnToOriginal(cluster);
            }
            else if (oldContainer != null && oldContainer != targetContainer)
            {
                oldContainer.ClearBuffers();
            }

            lastHovered?.ClearPlaceholder();
        }
    }
}