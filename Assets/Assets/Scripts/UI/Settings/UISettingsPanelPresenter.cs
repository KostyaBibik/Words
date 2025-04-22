using Core.GameState;
using Core.GameState.States;
using Core.Services;
using Core.Systems;
using Cysharp.Threading.Tasks;
using Enums;
using UI.Abstract;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace UI.Settings
{
    public class UISettingsPanelPresenter : UIPresenter<UISettingsPanelView>
    {
        private IGameSessionCleaner _sessionCleaner;
        private IGameStateMachine _stateMachine;
        private UIGameplayPresenter _gameplayPresenter;
        private IAudioService _audioService;

        public UISettingsPanelPresenter(UISettingsPanelView view) : base(view)
        {
        }

        [Inject]
        public void Construct(
            IGameSessionCleaner sessionCleaner,
            IGameStateMachine stateMachine,
            UIGameplayPresenter gameplayPresenter,
            IAudioService audioService
        )
        {
            _sessionCleaner = sessionCleaner;
            _stateMachine = stateMachine;
            _gameplayPresenter = gameplayPresenter;
            _audioService = audioService;
        }

        public override void Initialize()
        {
            _view
                .OnReturnBtnClick
                .Subscribe(_ => OnReturnBtnClick())
                .AddTo(_view);
            
            _view
                .OnMenuBtnClick
                .Subscribe(_ => OnMenuBtnClick())
                .AddTo(_view);
            
            Hide();
        }

        private void OnMenuBtnClick()
        {
            PlayAudioClick();
            _stateMachine.SwitchState<MainMenuState>().Forget();
            Hide(false);
            _sessionCleaner.Cleanup();
        }

        private void OnReturnBtnClick()
        {
            PlayAudioClick();
            Hide(false);
            _gameplayPresenter.Show(false);
        }
        
        private void PlayAudioClick() =>
            _audioService.PlaySound(ESoundType.UI_Click);
    }
}