using Scripts.Enums;
using UniRx;

namespace UI.Flow
{
    public interface IUIFlowManager
    {
        public void TrackProgress(IReadOnlyReactiveProperty<ELoadPhase> loadingPhase);
        public void ShowLoadingScreen();
        public void ShowMainMenuScreen();
        public void ShowErrorScreen(string message);
    }
}