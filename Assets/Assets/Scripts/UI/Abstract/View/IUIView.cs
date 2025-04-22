using System;
using Cysharp.Threading.Tasks;
using UniRx;

namespace UI.Abstract
{
    public interface IUIView : IDisposable
    {
        ReactiveCommand<Unit> OnInit { get; }
        ReactiveCommand<Unit> OnShow { get; }
        ReactiveCommand<Unit> OnHide { get; }
        UniTask Show(bool instant = true);
        UniTask Hide(bool instant = true);
    }
}