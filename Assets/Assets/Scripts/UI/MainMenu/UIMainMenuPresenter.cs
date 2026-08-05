using System;
using UI.Abstract;
using UniRx;
using VYandexTools.Localization.Scripts;

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
            _view.SetTitle(Loc.Text(LocalizationKey.menu_title, "СЛОВА\nИЗ КУСОЧКОВ"));
            Hide();
        }

        public void SetProgressText(int progress) =>
            _view.ProgressText.text = Loc.Format(LocalizationKey.menu_progress_format, "Current Level: {0}", progress);
    }
}