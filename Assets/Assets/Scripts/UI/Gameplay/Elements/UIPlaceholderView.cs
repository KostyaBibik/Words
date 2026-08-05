using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Juice;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    [RequireComponent(typeof(RectTransform))]
    public class UIPlaceholderView : MonoBehaviour
    {
        [Header("Juice")]
        [SerializeField] private float _appearDuration = 0.16f;
        [SerializeField] private float _fromScale = 0.7f;
        [Tooltip("Optional graphic that breathes while the placeholder is visible.")]
        [SerializeField] private Graphic _pulseGraphic;
        [SerializeField] private float _pulseAmount = 0.18f;
        [SerializeField] private float _pulseSpeed = 5f;

        private RectTransform _rectTransform;
        private Vector2 _originalSizeDelta;
        private Vector3 _baseScale = Vector3.one;
        private CancellationTokenSource _cts;
        private float _baseAlpha = 1f;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalSizeDelta = _rectTransform.sizeDelta;
            _baseScale = _rectTransform.localScale;

            if (_pulseGraphic != null)
                _baseAlpha = _pulseGraphic.color.a;

            gameObject.SetActive(false);
        }

        private void OnDestroy() => Cancel();

        private void Update()
        {
            if (_pulseGraphic == null || _pulseAmount <= 0f)
                return;

            var breathing = 1f - _pulseAmount + Mathf.Abs(Mathf.Sin(Time.unscaledTime * _pulseSpeed)) * _pulseAmount;
            var color = _pulseGraphic.color;

            _pulseGraphic.color = new Color(color.r, color.g, color.b, _baseAlpha * breathing);
        }

        public void Activate(RectTransform source)
        {
            _rectTransform.sizeDelta = source.sizeDelta;

            gameObject.SetActive(true);

            Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            _rectTransform.localScale = _baseScale * _fromScale;

            UITween.Scale(_rectTransform, _baseScale, _appearDuration, EEase.OutBack, _cts.Token)
                .SuppressCancellationThrow()
                .Forget();
        }

        public void Deactivate()
        {
            Cancel();

            _rectTransform.sizeDelta = _originalSizeDelta;
            _rectTransform.localScale = _baseScale;

            gameObject.SetActive(false);
        }

        private void Cancel()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
