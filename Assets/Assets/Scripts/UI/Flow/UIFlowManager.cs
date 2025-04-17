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

        private bool IsLoading { get; set; }

        public void Initialize()
        {
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

        public async UniTask WaitUntilReady()
        {
            await UniTask.WaitWhile(() => IsLoading);
        }
    }
}