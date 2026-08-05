using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Juice
{
    /// <summary>
    /// Press/release feedback for any clickable UI element.
    /// Sits alongside LeanButton (both receive the pointer events) so no button wiring changes.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class UIButtonJuice : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _pressedScale = 0.93f;
        [SerializeField] private float _pressDuration = 0.08f;
        [SerializeField] private float _releaseDuration = 0.24f;
        [Tooltip("Idle breathing to draw the eye to the primary action. 0 disables it.")]
        [SerializeField] private float _idlePulse;
        [SerializeField] private float _idlePulseSpeed = 1.6f;

        private Vector3 _baseScale;
        private bool _pressed;
        private CancellationTokenSource _cts;

        private void Awake()
        {
            if (_target == null)
                _target = transform;

            _baseScale = _target.localScale;
        }

        private void OnEnable()
        {
            _pressed = false;
            _target.localScale = _baseScale;
        }

        private void OnDisable()
        {
            CancelRunning();
            _target.localScale = _baseScale;
        }

        private void OnDestroy() => CancelRunning();

        private void Update()
        {
            if (_pressed || _idlePulse <= 0f)
                return;

            var pulse = 1f + Mathf.Sin(Time.unscaledTime * _idlePulseSpeed) * _idlePulse;
            _target.localScale = _baseScale * pulse;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _pressed = true;
            Play(UITween.Scale(_target, _baseScale * _pressedScale, _pressDuration, EEase.OutQuad, NewToken()));
        }

        public void OnPointerUp(PointerEventData eventData) => Release();

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_pressed)
                Release();
        }

        private void Release()
        {
            _pressed = false;
            Play(UITween.Scale(_target, _baseScale, _releaseDuration, EEase.OutBack, NewToken()));
        }

        private void Play(UniTask task) => task.SuppressCancellationThrow().Forget();

        private CancellationToken NewToken()
        {
            CancelRunning();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            return _cts.Token;
        }

        private void CancelRunning()
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _cts = null;
        }
    }
}
