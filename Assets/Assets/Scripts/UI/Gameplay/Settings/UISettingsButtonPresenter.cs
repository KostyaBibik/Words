using Core.Services;
using Enums;
using UI.Abstract;
using UI.Settings;
using UniRx;
using Zenject;

namespace UI.Gameplay.Settings
{
    public class UISettingsButtonPresenter : UIPresenter<UISettingsButtonView>
    {
        private UISettingsPanelPresenter _panelPresenter;
        private UIMainMenuPresenter _menuPresenter;
        private IAudioService _audioService;
        
        public UISettingsButtonPresenter(UISettingsButtonView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            UISettingsPanelPresenter panelPresenter,
            UIMainMenuPresenter menuPresenter,
            IAudioService audioService
        )
        {
            _panelPresenter = panelPresenter;
            _menuPresenter = menuPresenter;
            _audioService = audioService;
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
            PlayAudioClick();
            _menuPresenter.Hide(false);
            _panelPresenter.Show(false);
        }
        
        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
    }
}