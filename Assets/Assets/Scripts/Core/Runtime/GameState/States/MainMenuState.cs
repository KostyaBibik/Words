using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Flow;
using UI.Gameplay;
using UniRx;
using Zenject;

namespace Core.GameState
{
    public sealed class MainMenuState : IGameState
    {
        [Inject] private readonly IGameStateMachine _gameStateMachine;
        [Inject] private readonly UIMainMenuPresenter _mainMenuPresenter;
        [Inject] private readonly IUIFlowManager _uiFlowManager;
        [Inject] private readonly IGameDataRepository _dataRepository;
        
        private readonly CompositeDisposable _disposable = new();

        public async UniTask Enter()
        {
            _uiFlowManager.ShowMainMenuScreen();
            _mainMenuPresenter
                .OnStartPlayBtnClick
                .Subscribe(_ => OnStartPlayBtnClick())
                .AddTo(_disposable);

            var progressLevel = _dataRepository.CurrentLevel.id;
            _mainMenuPresenter.SetProgressText(progressLevel);
                
            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _mainMenuPresenter.Hide();
            _disposable?.Clear();
            
            await UniTask.CompletedTask;
        }

        private void OnStartPlayBtnClick() => 
            _gameStateMachine.SwitchState<LevelGenerationState>().Forget();
    }
}