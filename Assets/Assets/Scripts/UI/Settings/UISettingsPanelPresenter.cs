using Core.Services;
using Enums;
using UI.Abstract;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace UI.Settings
{
    public class UISettingsPanelPresenter : UIPresenter<UISettingsPanelView>
    {
        private UIMainMenuPresenter _menuPresenter;
        private IAudioService _audioService;

        public UISettingsPanelPresenter(UISettingsPanelView view) : base(view)
        {
        }

        [Inject]
        public void Construct(UIMainMenuPresenter menuPresenter, IAudioService audioService)
        {
            _menuPresenter = menuPresenter;
            _audioService = audioService;
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
            PlayAudioClick();
            Hide(false);
            _menuPresenter.Show(false);
        }

        private void OnSwapSoundsStatus()
        {
            _view.SwapSoundSprite();
            _audioService.SwapSoundsActiveStatus();
            PlayAudioClick();
        }
        
        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
    }
}