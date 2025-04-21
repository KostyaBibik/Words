using Cysharp.Threading.Tasks;
using UI.Flow;
using UI.Gameplay;
using UniRx;

namespace Core.GameState.States
{
    public class MainMenuState : IGameState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly UIMainMenuPresenter _mainMenuPresenter;
        private readonly IUIFlowManager _uiFlowManager;
        private readonly CompositeDisposable _compositeDisposable = new();
        
        public MainMenuState(
            IGameStateMachine gameStateMachine,
            UIMainMenuPresenter mainMenuPresenter,
            IUIFlowManager uiFlowManager
        )
        {
            _gameStateMachine = gameStateMachine;
            _mainMenuPresenter = mainMenuPresenter;
            _uiFlowManager = uiFlowManager;
        }

        public async UniTask Enter()
        {
            _uiFlowManager.ShowMainMenuScreen();
            _mainMenuPresenter
                .OnStartPlayBtnClick
                .Subscribe(_ => OnStartPlayBtnClick())
                .AddTo(_compositeDisposable);
                
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _mainMenuPresenter.Hide();
            _compositeDisposable?.Clear();
            
            await UniTask.CompletedTask;
        }

        private void OnStartPlayBtnClick()
            => _gameStateMachine.SwitchState<LevelGenerationState>().Forget();
    }
}