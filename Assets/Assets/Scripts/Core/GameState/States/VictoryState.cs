using Cysharp.Threading.Tasks;
using UI.Victory;
using UniRx;

namespace Core.GameState.States
{
    public class VictoryState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly UIVictoryPresenter _victoryPresenter;
        
        private readonly CompositeDisposable _disposable = new();

        public VictoryState(
            IGameStateMachine gameStateMachine,
            UIVictoryPresenter victoryPresenter
        )
        {
            _gameStateMachine = gameStateMachine;
            _victoryPresenter = victoryPresenter;
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
            
            await UniTask.CompletedTask;
        }
        
        public async UniTask Exit()
        {
            _victoryPresenter.Hide();
            _disposable?.Clear();
            
            await UniTask.CompletedTask;
        }
        
        private void OnContinueBtnClick()
            => _gameStateMachine.SwitchState<LevelGenerationState>().Forget();
        
        private void OnMenuBtnClick()
            => _gameStateMachine.SwitchState<LevelGenerationState>().Forget();
    }
}