using Assets.Scripts.Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace Core.GameState.States
{
    public class GameplayState : IGameState
    {
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;
        [Inject] private readonly IValidationService _validationService;
        
        private readonly IGameStateMachine _stateMachine;
        private readonly CompositeDisposable _disposable = new();

        public GameplayState(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public async UniTask Enter()
        {
            _gameplayPresenter.Show();

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
            _gameplayPresenter.Hide();
            
            await _stateMachine.SwitchState<VictoryState>();
        }

        public async UniTask Exit()
        {
            _gameplayPresenter.Hide();
            _disposable?.Clear();
            
            await UniTask.CompletedTask;
        }
    }
}