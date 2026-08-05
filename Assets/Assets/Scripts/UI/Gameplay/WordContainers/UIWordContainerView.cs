using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Abstract;
using UI.Gameplay.WordContainers;
using UI.Juice;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Gameplay.Elements
{
    public sealed class UIWordContainerView : UIView, IClusterDropZone
    {
        [SerializeField] private UILetterSlotView _letterSlotPrefab;

        [Header("Solved Feedback")]
        [Tooltip("Row background tinted when the assembled word is correct. Falls back to this object's Image.")]
        [SerializeField] private Graphic _background;
        [SerializeField] private Color _solvedColor = new(0.34f, 0.82f, 0.42f);
        [SerializeField] private float _tintDuration = 0.22f;

        private UIWordContainerPresenter _presenter;
        private CancellationTokenSource _feedbackCts;
        private Color _baseColor = Color.white;
        private bool _baseColorCaptured;
        private bool _isSolved;

        private readonly Subject<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            _onTryDrop = new();

        public UILetterSlotView LetterSlotPrefab => _letterSlotPrefab;

        public UIWordContainerPresenter Presenter => _presenter;

        public IObservable<(UIClusterElementView cluster, PointerEventData eventData, UniTaskCompletionSource<bool> tcs)>
            OnTryDrop => _onTryDrop;

        protected override void Awake()
        {
            base.Awake();

            if (_background == null)
                _background = GetComponent<Graphic>();

            CaptureBaseColor();
        }

        public void Initialize(UIWordContainerPresenter presenter) =>
            _presenter = presenter;

        public async UniTask<bool> TryDrop(UIClusterElementView cluster, PointerEventData eventData)
        {
            var taskSource = new UniTaskCompletionSource<bool>();
            _onTryDrop.OnNext((cluster, eventData, taskSource));
            return await taskSource.Task;
        }

        /// <summary>Green tint + pop the moment the row holds a correct word; reverts when it is broken up.</summary>
        public void SetSolved(bool isSolved)
        {
            if (_isSolved == isSolved || _background == null)
                return;

            _isSolved = isSolved;
            CaptureBaseColor();

            var token = NewToken();

            UITween.TintGraphic(_background, isSolved ? _solvedColor : _baseColor, _tintDuration, EEase.OutQuad, token)
                .SuppressCancellationThrow()
                .Forget();

            if (isSolved)
                UITween.Punch(transform, 0.12f, 0.3f, token).SuppressCancellationThrow().Forget();
        }

        public void PlayHintPulse()
        {
            var token = NewToken();

            UITween.Punch(transform, 0.14f, 0.42f, token).SuppressCancellationThrow().Forget();
        }

        private void CaptureBaseColor()
        {
            if (_baseColorCaptured || _background == null)
                return;

            _baseColor = _background.color;
            _baseColorCaptured = true;
        }

        private CancellationToken NewToken()
        {
            _feedbackCts?.Cancel();
            _feedbackCts?.Dispose();
            _feedbackCts = CancellationTokenSource.CreateLinkedTokenSource(this.GetCancellationTokenOnDestroy());

            return _feedbackCts.Token;
        }

        private void OnDestroy()
        {
            _feedbackCts?.Cancel();
            _feedbackCts?.Dispose();
            _feedbackCts = null;

            _onTryDrop?.OnCompleted();
        }
    }
}
