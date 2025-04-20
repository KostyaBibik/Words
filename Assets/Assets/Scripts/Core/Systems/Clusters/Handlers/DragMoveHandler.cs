using UI.Gameplay.Elements;
using UI.Gameplay.Utils;
using UnityEngine;

namespace UI.Gameplay
{
    public class DragMoveHandler
    {
        private readonly RectTransform _canvasRectTransform;

        public DragMoveHandler(RectTransform canvasRectTransform)
        {
            _canvasRectTransform = canvasRectTransform;
        }

        public void UpdateClusterPosition(UIClusterElementView cluster, Vector2 screenPosition, Vector3 offset)
        {
            var worldPoint = UIRectTransformConverter.ScreenToWorld(_canvasRectTransform, screenPosition);

            cluster.transform.position = worldPoint + offset;
        }

        public (UIWordContainerView, bool) HandleRaycastAndPlaceholder(
            UIClusterElementView cluster,
            Vector2 screenPosition,
            UIWordContainerView lastHovered)
        {
            var results = UIRaycastHelper.RaycastAtScreenPosition(screenPosition);

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<UIWordContainerView>(out var container))
                {
                    if (lastHovered != container)
                    {
                        lastHovered?.ClearPlaceholder();
                    }

                    var localPoint = UIRectTransformConverter.ScreenToLocal((RectTransform)container.transform, screenPosition);
                    var targetSlotIndex = container.CalculateSlotIndexFromPosition(localPoint);
                    var startIndex = targetSlotIndex - cluster.GrabbedLetterIndex;

                    container.ShowPlaceholder(cluster, startIndex);
                    return (container, true);
                }
            }

            return (lastHovered, false);
        }
    }
}
