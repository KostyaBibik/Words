using System;
using UniRx;

namespace Assets.Scripts.UI.Core
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