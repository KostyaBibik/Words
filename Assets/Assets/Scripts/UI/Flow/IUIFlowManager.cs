namespace UI.Flow
{
    public interface IUIFlowManager
    {
        public void ShowLoadingScreen();
        public void ShowMainMenuScreen();
        public void ShowErrorScreen(string message);
    }
}