using System;
using Zenject;

namespace UI.Abstract
{
    public abstract class UIPresenter<TView> : IUIPresenter, IInitializable, IDisposable 
        where TView : IUIView
    {
        protected readonly TView _view;

        protected UIPresenter(TView view)
        {
            _view = view;
        }

        public virtual void Initialize()
        {
        }

        public void Show(bool instant = true)
        {
            BeforeShow();
            
            _view.Show(instant);
        }
        
        protected virtual void BeforeShow() {}

        public void Hide(bool instant = true)
        {
            BeforeHide();
            
            _view.Hide(instant);
        }
        protected virtual void BeforeHide() {}

        public virtual void Dispose()
        {
            _view?.Dispose();
        }
    }
}