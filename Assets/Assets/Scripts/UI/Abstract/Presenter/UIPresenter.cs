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

        public void Show() => _view.Show();
        public void Hide() => _view.Hide();

        public virtual void Dispose()
        {
            _view?.Dispose();
        }
    }
}