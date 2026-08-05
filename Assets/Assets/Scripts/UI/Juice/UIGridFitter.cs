using UnityEngine;
using UnityEngine.UI;

namespace UI.Juice
{
    /// <summary>
    /// Scales and re-centres a layout group so its children always fit inside its own rect.
    /// The word grid sizes itself from its children (ContentSizeFitter on every row,
    /// childControlHeight off on the parent), so with enough words it overflows and paints
    /// over whatever sits below it. Layout metrics are useless here — a group that does not
    /// control child height reports a meaningless preferred size — so the real bounding box
    /// of the children is measured instead. Uniform scaling keeps the original proportions
    /// and needs no changes to the row or slot prefabs.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform))]
    public sealed class UIGridFitter : MonoBehaviour
    {
        [Tooltip("Breathing room so content never touches the band edges.")]
        [SerializeField] private float _padding = 24f;
        [Tooltip("Content is only ever scaled down, never up past this.")]
        [SerializeField] private float _maxScale = 1f;
        [SerializeField] private float _minScale = 0.2f;

        private RectTransform _rect;
        private Vector2 _basePosition;
        private bool _baseCaptured;

        private RectTransform Rect
        {
            get
            {
                if (_rect == null)
                    _rect = (RectTransform)transform;

                return _rect;
            }
        }

        private void Awake() => CaptureBase();

        private void CaptureBase()
        {
            if (_baseCaptured)
                return;

            _basePosition = Rect.anchoredPosition;
            _baseCaptured = true;
        }

        public void Fit()
        {
            CaptureBase();

            var rect = Rect;

            // Measure from a clean state, otherwise every call compounds the previous shrink.
            rect.localScale = Vector3.one;
            rect.anchoredPosition = _basePosition;
            LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

            var bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(rect);

            if (bounds.size.x <= 0.01f || bounds.size.y <= 0.01f)
                return;

            var availableWidth = rect.rect.width - _padding;
            var availableHeight = rect.rect.height - _padding;

            if (availableWidth <= 0f || availableHeight <= 0f)
                return;

            var scale = Mathf.Min(_maxScale,
                Mathf.Min(availableWidth / bounds.size.x, availableHeight / bounds.size.y));

            scale = Mathf.Clamp(scale, _minScale, _maxScale);

            rect.localScale = new Vector3(scale, scale, 1f);

            // bounds.center is in this rect's local space; cancel it out so the grid
            // sits in the middle of its band no matter how the group aligned it.
            rect.anchoredPosition = _basePosition - (Vector2)bounds.center * scale;
        }
    }
}
