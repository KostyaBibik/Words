using Scripts.Enums;
using UI.ErrorLoading;
using UI.Gameplay;
using UI.Loading;
using UniRx;
using Zenject;

namespace UI.Flow
{
    public class UIFlowManager : IUIFlowManager, IInitializable
    {
        [Inject] private readonly UILoadingPresenter _loadingPresenter;
        [Inject] private readonly UIMainMenuPresenter _mainMenuPresenter;
        [Inject] private readonly UIErrorLoadingPresenter _errorLoadingPresenter;
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;

        public void Initialize()
        {
            _gameplayPresenter.Hide();
        }

        public void TrackProgress(IReadOnlyReactiveProperty<ELoadPhase> loadingPhase) =>
            _loadingPresenter.TrackProgress(loadingPhase);
        
        public void ShowLoadingScreen()
        {
            _mainMenuPresenter.Hide();
            _errorLoadingPresenter.Hide();
            
            _loadingPresenter.Show();
        }

        public void ShowMainMenuScreen()
        {
            _loadingPresenter.Hide();
            _errorLoadingPresenter.Hide();

            _mainMenuPresenter.Show(false);
        }

        public void ShowErrorScreen(string message)
        {
            _mainMenuPresenter.Hide();
            _loadingPresenter.Hide();

            _errorLoadingPresenter.Show();
            _errorLoadingPresenter.UpdateText(message);
        }
    }
}