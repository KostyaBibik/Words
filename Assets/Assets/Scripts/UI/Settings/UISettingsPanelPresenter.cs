using UI.Abstract;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace UI.Settings
{
    public class UISettingsPanelPresenter : UIPresenter<UISettingsPanelView>
    {
        [Inject] private UIMainMenuPresenter _menuPresenter;

        public UISettingsPanelPresenter(UISettingsPanelView view) : base(view)
        {
        }

        public override void Initialize()
        {
            _view
                .OnReturnBtnClick
                .Subscribe(_ => OnReturnBtnClick())
                .AddTo(_view);

            _view
                .OnSoundsBtnClick
                .Subscribe(_ => OnSwapSoundsStatus())
                .AddTo(_view);
            
            Hide();
        }

        private void OnReturnBtnClick()
        {
            Hide(false);
            _menuPresenter.Show(false);
        }

        private void OnSwapSoundsStatus()
        {
            _view.SwapSoundSprite();
        }
    }
}