using UniRx;
using UnityEngine;

namespace UI.Abstract
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIView : MonoBehaviour, IUIView
    {
        [SerializeField] protected CanvasGroup _canvasGroup;

        public ReactiveCommand<Unit> OnInit { get; } = new();
        public ReactiveCommand<Unit> OnShow { get; } = new();
        public ReactiveCommand<Unit> OnHide { get; } = new();

        protected virtual void Awake()
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }
        }

        public virtual void Show()
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            OnShow?.Execute(Unit.Default);
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            OnHide?.Execute(Unit.Default);
        }

        public virtual void Dispose()
        {
            OnInit?.Dispose();
            OnShow?.Dispose();
            OnHide?.Dispose();
        }
    }
}