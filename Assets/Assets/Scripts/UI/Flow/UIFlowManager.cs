using Scripts.Enums;
using UI.ErrorLoading;
using UI.Gameplay;
using UI.Loading;
using UniRx;

namespace UI.Flow
{
    public class UIFlowManager : IUIFlowManager
    {
        private UILoadingPresenter _loadingPresenter;
        private UIMainMenuPresenter _mainMenuPresenter;
        private UIErrorLoadingPresenter _errorLoadingPresenter;

        public void Init(
            UILoadingPresenter loadingPresenter,
            UIMainMenuPresenter mainMenuPresenter,
            UIErrorLoadingPresenter errorLoadingPresenter)
        {
            _loadingPresenter = loadingPresenter;
            _mainMenuPresenter = mainMenuPresenter;
            _errorLoadingPresenter = errorLoadingPresenter;
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