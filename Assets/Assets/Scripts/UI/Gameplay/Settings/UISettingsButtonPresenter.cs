using UI.Abstract;
using UI.Settings;
using UniRx;
using Zenject;

namespace UI.Gameplay.Settings
{
    public class UISettingsButtonPresenter : UIPresenter<UISettingsButtonView>
    {
        [Inject] private UISettingsPanelPresenter _panelPresenter;
        [Inject] private UIMainMenuPresenter _menuPresenter;
        
        public UISettingsButtonPresenter(UISettingsButtonView view) : base(view)
        {
        }
        
        public override void Initialize()
        {
            _view
                .OnBtnClick
                .Subscribe(_ => OnButtonClick())
                .AddTo(_view);
        }

        private void OnButtonClick()
        {
            _menuPresenter.Hide(false);
            _panelPresenter.Show(false);
        }
    }
}