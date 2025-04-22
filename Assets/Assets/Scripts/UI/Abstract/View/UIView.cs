using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace UI.Abstract
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected CanvasGroup _canvasGroup;
        
        private float _fadeDuration = 0.3f;
        
        public ReactiveCommand<Unit> OnInit { get; } = new();
        public ReactiveCommand<Unit> OnShow { get; } = new();
        public ReactiveCommand<Unit> OnHide { get; } = new();

        private CancellationTokenSource _fadeCts;
        
        protected virtual void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public virtual async UniTask Show(bool instant = true)
        {
            _fadeCts?.Cancel();
            _fadeCts = new CancellationTokenSource();
            
            if (instant)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                OnShow?.Execute(Unit.Default);
                return;
            }

            try
            {
                while (_canvasGroup.alpha < 1f)
                {
                    _canvasGroup.alpha += Time.deltaTime / _fadeDuration;
                    await UniTask.Yield(_fadeCts.Token);
                }
                
                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                OnShow?.Execute(Unit.Default);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public virtual async UniTask Hide(bool instant = true)
        {
            _fadeCts?.Cancel();
            _fadeCts = new CancellationTokenSource();
            
            if (instant)
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                OnHide?.Execute(Unit.Default);
                return;
            }

            try
            {
                while (_canvasGroup.alpha > 0f)
                {
                    _canvasGroup.alpha -= Time.deltaTime / _fadeDuration;
                    await UniTask.Yield(_fadeCts.Token);
                }
                
                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                OnHide?.Execute(Unit.Default);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public virtual void Dispose()
        {
            OnInit?.Dispose();
            OnShow?.Dispose();
            OnHide?.Dispose();
        }
    }
}