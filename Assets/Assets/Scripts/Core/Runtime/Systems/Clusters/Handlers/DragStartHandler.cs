using UI.Gameplay.Elements;
using UnityEngine;

namespace UI.Gameplay
{
    public sealed class DragStartHandler
    {
        private readonly RectTransform _canvasRectTransform;
        private readonly Transform _dragLayer;

        public DragStartHandler(RectTransform canvasRectTransform, Transform dragLayer)
        {
            _canvasRectTransform = canvasRectTransform;
            _dragLayer = dragLayer;
        }

        public Vector3 BeginDrag(UIClusterElementView cluster)
        {
            var rt = cluster.GetComponent<RectTransform>();
            rt.SetParent(_dragLayer);
            rt.SetAsLastSibling();

            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _canvasRectTransform, Input.mousePosition, null, out var worldPoint);

            return rt.position - worldPoint;
        }
    }
}