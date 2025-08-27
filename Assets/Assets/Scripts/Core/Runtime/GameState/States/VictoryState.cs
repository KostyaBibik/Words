using Core.Services;
using Core.Systems;
using Cysharp.Threading.Tasks;
using UI.Victory;
using UniRx;
using Zenject;

namespace Core.GameState
{
    public sealed class VictoryState : IGameState
    {
        [Inject] private readonly IGameStateMachine _gameStateMachine;
        [Inject] private readonly UIVictoryPresenter _victoryPresenter;
        [Inject] private readonly IGameDataRepository _gameDataRepository;
        [Inject] private readonly IGameSessionCleaner _sessionCleaner;

        private readonly CompositeDisposable _disposable = new();

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