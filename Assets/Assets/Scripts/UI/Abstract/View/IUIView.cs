using System;
using UniRx;

namespace UI.Abstract
{
    public interface IUIView : IDisposable
    {
        ReactiveCommand<Unit> OnInit { get; }
        ReactiveCommand<Unit> OnShow { get; }
        ReactiveCommand<Unit> OnHide { get; }
        void Show();
        void Hide();
    }
}