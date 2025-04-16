using Assets.Scripts.Architecture.DI;

namespace Assets.Scripts.Infrastructure
{
    public class AppStartup 
    {
        private readonly GameInstaller _gameInstaller;
    
        public AppStartup(GameInstaller gameInstaller) {
            _gameInstaller = gameInstaller;
            Initialize();
        }

        private void Initialize() 
        {
            // 1. Загружаем уровни (асинхронно)
            // 2. Стартуем первую сцену/состояние
        }
    }
}