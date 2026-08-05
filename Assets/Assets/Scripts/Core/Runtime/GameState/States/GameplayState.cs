using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Ads;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace Core.GameState
{
    public sealed class GameplayState : IGameState
    {
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;
        [Inject] private readonly IValidationService _validationService;
        [Inject] private readonly IGameStateMachine _stateMachine;
        [Inject] private readonly IAdsIconView _adsIconView;

        private readonly CompositeDisposable _disposable = new();

        public async UniTask Enter()
        {
            _gameplayPresenter.Show(false);
            _adsIconView.Show();

            SubscribeToCorrectValidation();

            await UniTask.CompletedTask;
        }

        private void SubscribeToCorrectValidation()
        {
            _validationService
                .ValidationStatus
                .Where(status => status)
                .Subscribe(_ => OnCorrectValidation())
                .AddTo(_disposable);
        }

        private async void OnCorrectValidation()
        {
            _gameplayPresenter.Hide(false);
            
            await _stateMachine.SwitchState<VictoryState>();
        }

        public async UniTask Exit()
        {
            _adsIconView.Hide();
            _gameplayPresenter.Hide(false);
            _disposable?.Clear();

            await UniTask.CompletedTask;
        }
    }
}