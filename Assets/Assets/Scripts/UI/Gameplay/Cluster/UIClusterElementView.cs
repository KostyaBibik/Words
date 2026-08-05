using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Juice;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public class UIClusterElementView : UIView, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Transform _frame;

        [Header("Drag Juice")]
        [SerializeField] private float _grabbedScale = 1.12f;
        [SerializeField] private float _grabbedTilt = 4f;
        [SerializeField] private float _grabDuration = 0.14f;
        [SerializeField] private float _dropDuration = 0.22f;
        [Tooltip("Optional shadow that fades in while the cluster is lifted.")]
        [SerializeField] private Graphic _liftShadow;
        [SerializeField] private float _liftShadowAlpha = 0.35f;

        private UIClusterElementPresenter _presenter;
        private CancellationTokenSource _juiceCts;
        private Vector3 _restScale = Vector3.one;
        private bool _restScaleCaptured;

        private readonly Subject<PointerEventData> _onDragStarted = new();
        private readonly Subject<Vector2> _onDragging = new();
        private readonly Subject<PointerEventData> _onDragEnded = new();

        public UIClusterElementPresenter Presenter => _presenter;
        public IObservable<PointerEventData> OnDragStarted => _onDragStarted.AsObservable();
        public IObservable<Vector2> OnDragging => _onDragging.AsObservable();
        public IObservable<PointerEventData> OnDragEnded => _onDragEnded.AsObservable();

        protected override void Awake()
        {
            base.Awake();
            CaptureRestScale();

            if (_liftShadow != null)
                SetShadowAlpha(0f);
        }

        public void Initialize(UIClusterElementPresenter presenter) =>
            _presenter = presenter;

        public void OnBeginDrag(PointerEventData eventData)
        {
            _onDragStarted.OnNext(eventData);
            PlayGrabJuice();
        }

        public void OnDrag(PointerEventData eventData) =>
            _onDragging.OnNext(eventData.position);

        public void OnEndDrag(PointerEventData eventData)
        {
            _onDragEnded.OnNext(eventData);
            PlayDropJuice();
        }

        public void UpdateFrame()
            => _frame.SetAsLastSibling();

        /// <summary>Bounce played when the cluster snaps into a word slot.</summary>
        public void PlaySnapJuice()
        {
            CaptureRestScale();
            transform.localScale = _restScale;

            UITween.Punch(transform, 0.22f, 0.3f, NewToken())
                .SuppressCancellationThrow()
                .Forget();
        }

        /// <summary>Attention pulse used by the hint system to point at a cluster.</summary>
        public void PlayHintPulse()
        {
            CaptureRestScale();

            var token = NewToken();

            HintPulseAsync(token).SuppressCancellationThrow().Forget();
        }

        private async UniTask HintPulseAsync(CancellationToken token)
        {
            const int pulses = 3;

            for (var i = 0; i < pulses; i++)
            {
                if (this == null)
                    return;

                await UITween.Scale(transform, _restScale * 1.25f, 0.16f, EEase.OutBack, token);
                await UITween.Scale(transform, _restScale, 0.16f, EEase.OutQuad, token);
            }
        }

        /// <summary>Wobble played when the cluster is rejected and returns to the panel.</summary>
        public void PlayRejectJuice()
        {
            UITween.Wobble(transform, 8f, 0.45f, NewToken())
                .SuppressCancellationThrow()
                .Forget();
        }

        private void CaptureRestScale()
        {
            if (_restScaleCaptured)
                return;

            _restScale = transform.localScale;
            _restScaleCaptured = true;
        }

        private void PlayGrabJuice()
        {
            CaptureRestScale();

            var token = NewToken();

            UniTask.WhenAll(
                    UITween.Scale(transform, _restScale * _grabbedScale, _grabDuration, EEase.OutBack, token),
                    UITween.RotateZ(transform, _grabbedTilt, _grabDuration, EEase.OutQuad, token),
                    FadeShadow(_liftShadowAlpha, _grabDuration, token))
                .SuppressCancellationThrow()
                .Forget();
        }

        private void PlayDropJuice()
        {
            var token = NewToken();

            UniTask.WhenAll(
                    UITween.Scale(transform, _restScale, _dropDuration, EEase.OutBack, token),
                    UITween.RotateZ(transform, 0f, _dropDuration, EEase.OutQuad, token),
                    FadeShadow(0f, _dropDuration, token))
                .SuppressCancellationThrow()
                .Forget();
        }

        private UniTask FadeShadow(float alpha, float duration, CancellationToken token)
        {
            if (_liftShadow == null)
                return UniTask.CompletedTask;

            var from = _liftShadow.color.a;

            return UITween.Value(from, alpha, duration, EEase.OutQuad, SetShadowAlpha, token);
        }

        private void SetShadowAlpha(float alpha)
        {
            var color = _liftShadow.color;
            _liftShadow.color = new Color(color.r, color.g, color.b, alpha);
        }

        private CancellationToken NewToken()
        {
            _juiceCts?.Cancel();
            _juiceCts?.Dispose();
            _juiceCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            return _juiceCts.Token;
        }

        private void OnDestroy()
        {
            _juiceCts?.Cancel();
            _juiceCts?.Dispose();
            _juiceCts = null;

            _presenter.Clear();
        }
    }
}
