namespace UI.Flow
{
    public interface IUIFlowManager
    {
        public void ShowLoadingScreen();
        public void ShowGameScreen();
        public void ShowErrorScreen(string message);
    }
}