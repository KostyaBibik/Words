using Scripts.Enums;
using UI.ErrorLoading;
using UI.Gameplay;
using UI.Loading;
using UniRx;

namespace UI.Flow
{
    public interface IUIFlowManager
    {
        public void TrackProgress(IReadOnlyReactiveProperty<ELoadPhase> loadingPhase);
        public void ShowLoadingScreen();
        public void ShowMainMenuScreen();
        public void ShowErrorScreen(string message);

        public void Init(
            UILoadingPresenter loadingPresenter,
            UIMainMenuPresenter mainMenuPresenter,
            UIErrorLoadingPresenter errorLoadingPresenter);
    }
}