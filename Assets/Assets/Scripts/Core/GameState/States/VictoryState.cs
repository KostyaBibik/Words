using Core.Services;
using Core.Systems;
using Cysharp.Threading.Tasks;
using UI.Victory;
using UniRx;

namespace Core.GameState.States
{
    public sealed class VictoryState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly UIVictoryPresenter _victoryPresenter;
        private readonly IGameDataRepository _gameDataRepository;
        private readonly IGameSessionCleaner _sessionCleaner;

        private readonly CompositeDisposable _disposable = new();

        public VictoryState(
            IGameStateMachine gameStateMachine,
            UIVictoryPresenter victoryPresenter,
            IGameDataRepository gameDataRepository,
            IGameSessionCleaner sessionCleaner
        )
        {
            _gameStateMachine = gameStateMachine;
            _victoryPresenter = victoryPresenter;
            _gameDataRepository = gameDataRepository;
            _sessionCleaner = sessionCleaner;
        }

        public async UniTask Enter()
        {
            _victoryPresenter.Show();

            _victoryPresenter
                .OnContinueBtnClick
                .Subscribe(_ => OnContinueBtnClick())
                .AddTo(_disposable);

            _victoryPresenter
                .OnMenuBtnClick
                .Subscribe(_ => OnMenuBtnClick())
                .AddTo(_disposable);

            _gameDataRepository.IncreaseLevel();

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _victoryPresenter.Hide();
            
            _disposable.Clear();
            _sessionCleaner.Cleanup();

            await UniTask.CompletedTask;
        }

        private void OnContinueBtnClick() =>
            _gameStateMachine.SwitchState<LevelGenerationState>().Forget();

        private void OnMenuBtnClick() =>
            _gameStateMachine.SwitchState<MainMenuState>().Forget();
    }

}