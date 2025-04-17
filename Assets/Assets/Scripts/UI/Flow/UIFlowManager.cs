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
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;
        [Inject] private readonly UIErrorLoadingPresenter _errorLoadingPresenter;

        private bool IsLoading { get; set; }

        public void Initialize()
        {
        }
        
        public void ShowLoadingScreen()
        {
            _gameplayPresenter.Hide();
            _errorLoadingPresenter.Hide();
            
            _loadingPresenterPresenter.Show();
        }

        public void ShowGameScreen()
        {
            _loadingPresenterPresenter.Hide();
            _errorLoadingPresenter.Hide();

            _gameplayPresenter.Show();
        }

        public void ShowErrorScreen(string message)
        {
            _gameplayPresenter.Hide();
            _loadingPresenterPresenter.Hide();

            _errorLoadingPresenter.Show();
        }

        public async UniTask WaitUntilReady()
        {
            await UniTask.WaitWhile(() => IsLoading);
        }
    }
}