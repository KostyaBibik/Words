using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Juice;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public class UILetterSlotView : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Color _occupiedColor = new Color(0.8f, 0f, 0.1f);
        [SerializeField] private Color _basedColor = Color.yellow;
        [SerializeField] private Color _placeholderColor = new Color(0.5f, 0f, 0.5f);

        [Header("References")]
        [SerializeField] private Image _background;

        [Header("Juice")]
        [SerializeField] private float _colorDuration = 0.14f;
        [SerializeField] private float _highlightScale = 1.1f;
        [SerializeField] private float _highlightDuration = 0.16f;

        private CancellationTokenSource _colorCts;
        private CancellationTokenSource _scaleCts;
        private Vector3 _baseScale = Vector3.one;
        private bool _isPlaceholder;

        public bool IsOccupied { get; private set; }
        public int Index { get; private set; }

        private void Awake() => _baseScale = transform.localScale;

        private void OnDestroy()
        {
            CancelColor();
            CancelScale();
        }

        public void Initialize(int index)
        {
            transform.name = $"LetterSlot_{index}";
            Index = index;
        }

        public void SetOccupied(bool flag)
        {
            IsOccupied = flag;

            // The slot is deactivated while occupied, so snap the colour instead of tweening into nothing.
            CancelColor();
            _background.color = flag ? _occupiedColor : _basedColor;

            gameObject.SetActive(!flag);

            if (!flag)
            {
                _isPlaceholder = false;
                transform.localScale = _baseScale;
            }
        }

        public void SetAsPlaceholder(bool isPlaceholder)
        {
            if (IsOccupied || _isPlaceholder == isPlaceholder)
                return;

            _isPlaceholder = isPlaceholder;

            TintTo(isPlaceholder ? _placeholderColor : _basedColor);
            ScaleTo(isPlaceholder ? _baseScale * _highlightScale : _baseScale);
        }

        private void TintTo(Color color)
        {
            CancelColor();

            if (!isActiveAndEnabled)
            {
                _background.color = color;
                return;
            }

            _colorCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            UITween.TintGraphic(_background, color, _colorDuration, EEase.OutQuad, _colorCts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }

        private void ScaleTo(Vector3 scale)
        {
            CancelScale();

            if (!isActiveAndEnabled)
            {
                transform.localScale = scale;
                return;
            }

            _scaleCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            UITween.Scale(transform, scale, _highlightDuration, EEase.OutBack, _scaleCts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }

        private void CancelColor()
        {
            _colorCts?.Cancel();
            _colorCts?.Dispose();
            _colorCts = null;
        }

        private void CancelScale()
        {
            _scaleCts?.Cancel();
            _scaleCts?.Dispose();
            _scaleCts = null;
        }
    }
}
