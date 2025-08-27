using System;
using UI.Abstract;
using UniRx;

namespace UI.Gameplay
{
    public sealed class UIMainMenuPresenter : UIPresenter<UIMainMenuView>
    {
        public IObservable<Unit> OnStartPlayBtnClick => _view.StartPlayBtn.OnClick.AsObservable();
        
        public UIMainMenuPresenter(UIMainMenuView view) : base(view) 
        {
        }

        public override void Initialize()
        {
            Hide();
        }
    }
}