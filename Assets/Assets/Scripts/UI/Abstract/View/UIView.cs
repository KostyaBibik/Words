using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UI.Juice;
using UniRx;
using UnityEngine;

namespace UI.Abstract
{
    public enum EViewTransition
    {
        PopScale = 0,
        Fade = 1,
        SlideUp = 2,
        SlideDown = 3
    }

    [RequireComponent(typeof(CanvasGroup))]
    public class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected CanvasGroup _canvasGroup;

        [Header("Transition")]
        [SerializeField] protected EViewTransition _transition = EViewTransition.PopScale;
        [Tooltip("Panel that gets scaled/moved. Falls back to this view's own RectTransform.")]
        [SerializeField] protected RectTransform _content;
        [SerializeField] protected float _showDuration = 0.34f;
        [SerializeField] protected float _hideDuration = 0.2f;
        [SerializeField] protected float _popFromScale = 0.82f;
        [SerializeField] protected float _slideDistance = 220f;

        public ReactiveCommand<Unit> OnInit { get; } = new();
        public ReactiveCommand<Unit> OnShow { get; } = new();
        public ReactiveCommand<Unit> OnHide { get; } = new();

        private CancellationTokenSource _transitionCts;
        private Vector2 _contentBasePosition;
        private Vector3 _contentBaseScale = Vector3.one;
        private bool _baseCaptured;

        protected RectTransform Content
        {
            get
            {
                if (_content == null)
                    _content = transform as RectTransform;

                return _content;
            }
        }

        protected virtual void Awake()
        {
            if (_canvasGroup == null)
                _canvasGroup = GetComponent<CanvasGroup>();

            CaptureBaseTransform();
        }

        private void CaptureBaseTransform()
        {
            if (_baseCaptured || Content == null)
                return;

            _contentBasePosition = Content.anchoredPosition;
            _contentBaseScale = Content.localScale;
            _baseCaptured = true;
        }

        public virtual async UniTask Show(bool instant = true)
        {
            CaptureBaseTransform();
            var token = RestartTransition();

            _canvasGroup.blocksRaycasts = true;

            if (instant)
            {
                ApplyRestState(1f);
                OnShow?.Execute(Unit.Default);
                return;
            }

            try
            {
                ApplyEnterState();

                await UniTask.WhenAll(
                    UITween.Fade(_canvasGroup, 1f, _showDuration * 0.75f, EEase.OutQuad, token),
                    AnimateContentIn(token));

                ApplyRestState(1f);
                OnShow?.Execute(Unit.Default);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public virtual async UniTask Hide(bool instant = true)
        {
            CaptureBaseTransform();
            var token = RestartTransition();

            _canvasGroup.blocksRaycasts = false;

            if (instant)
            {
                ApplyRestState(0f);
                OnHide?.Execute(Unit.Default);
                return;
            }

            try
            {
                await UniTask.WhenAll(
                    UITween.Fade(_canvasGroup, 0f, _hideDuration, EEase.InQuad, token),
                    AnimateContentOut(token));

                ApplyRestState(0f);
                OnHide?.Execute(Unit.Default);
            }
            catch (OperationCanceledException)
            {
            }
        }

        private CancellationToken RestartTransition()
        {
            _transitionCts?.Cancel();
            _transitionCts?.Dispose();
            _transitionCts = CancellationTokenSource.CreateLinkedTokenSource(
                this.GetCancellationTokenOnDestroy());

            return _transitionCts.Token;
        }

        private void ApplyEnterState()
        {
            _canvasGroup.alpha = 0f;

            var content = Content;

            if (content == null)
                return;

            switch (_transition)
            {
                case EViewTransition.PopScale:
                    content.localScale = _contentBaseScale * _popFromScale;
                    content.anchoredPosition = _contentBasePosition;
                    break;
                case EViewTransition.SlideUp:
                    content.anchoredPosition = _contentBasePosition + new Vector2(0f, -_slideDistance);
                    break;
                case EViewTransition.SlideDown:
                    content.anchoredPosition = _contentBasePosition + new Vector2(0f, _slideDistance);
                    break;
            }
        }

        private UniTask AnimateContentIn(CancellationToken token)
        {
            var content = Content;

            if (content == null)
                return UniTask.CompletedTask;

            switch (_transition)
            {
                case EViewTransition.PopScale:
                    return UITween.Scale(content, _contentBaseScale, _showDuration, EEase.OutBack, token);
                case EViewTransition.SlideUp:
                case EViewTransition.SlideDown:
                    return UITween.AnchoredMove(content, _contentBasePosition, _showDuration, EEase.OutBack, token);
                default:
                    return UniTask.CompletedTask;
            }
        }

        private UniTask AnimateContentOut(CancellationToken token)
        {
            var content = Content;

            if (content == null)
                return UniTask.CompletedTask;

            switch (_transition)
            {
                case EViewTransition.PopScale:
                    return UITween.Scale(content, _contentBaseScale * _popFromScale, _hideDuration, EEase.InQuad, token);
                case EViewTransition.SlideUp:
                    return UITween.AnchoredMove(content, _contentBasePosition + new Vector2(0f, -_slideDistance),
                        _hideDuration, EEase.InQuad, token);
                case EViewTransition.SlideDown:
                    return UITween.AnchoredMove(content, _contentBasePosition + new Vector2(0f, _slideDistance),
                        _hideDuration, EEase.InQuad, token);
                default:
                    return UniTask.CompletedTask;
            }
        }

        private void ApplyRestState(float alpha)
        {
            _canvasGroup.alpha = alpha;

            // A Fade transition never touches the transform, so leave layout-driven
            // elements (clusters, word rows) exactly where their layout group put them.
            if (_transition == EViewTransition.Fade)
                return;

            var content = Content;

            if (content == null)
                return;

            content.localScale = _contentBaseScale;
            content.anchoredPosition = _contentBasePosition;
        }

        public virtual void Dispose()
        {
            _transitionCts?.Cancel();
            _transitionCts?.Dispose();
            _transitionCts = null;

            OnInit?.Dispose();
            OnShow?.Dispose();
            OnHide?.Dispose();
        }
    }
}
