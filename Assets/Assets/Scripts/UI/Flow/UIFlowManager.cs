using Core.Services.Models;
using Cysharp.Threading.Tasks;
using UI.ErrorLoading;
using UI.Gameplay;
using UI.Loading;
using Zenject;

namespace UI.Flow
{
    public class UIFlowManager : IUIFlowManager, IInitializable
    {
        [Inject] private readonly UILoadingPresenter _loadingPresenterPresenter;
        [Inject] private readonly UIMainMenuPresenter _mainMenuPresenter;
        [Inject] private readonly UIErrorLoadingPresenter _errorLoadingPresenter;
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;

        public void Initialize()
        {
            _gameplayPresenter.Hide();
        }
        
        public void ShowLoadingScreen()
        {
            _mainMenuPresenter.Hide();
            _errorLoadingPresenter.Hide();
            
            _loadingPresenterPresenter.Show();
        }

        public void ShowMainMenuScreen()
        {
            _loadingPresenterPresenter.Hide();
            _errorLoadingPresenter.Hide();

            _mainMenuPresenter.Show();
        }

        public void ShowErrorScreen(string message)
        {
            _mainMenuPresenter.Hide();
            _loadingPresenterPresenter.Hide();

            _errorLoadingPresenter.Show();
        }
    }
}