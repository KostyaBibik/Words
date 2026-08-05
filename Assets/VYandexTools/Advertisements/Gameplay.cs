using Kimicu.YandexGames;

namespace Yandex
{
    /// <summary>
    /// Обёртка над gameplayStart/gameplayStop Яндекс SDK (требование модерации 1.19.3).
    /// Идемпотентна — защищает от двойных вызовов при пересечении событий фокуса и рекламы.
    /// </summary>
    public static class Gameplay
    {
        private static bool _active;

        public static void Start()
        {
            if (_active) return;
            _active = true;
            YandexGamesSdk.GameStart();
        }

        public static void Stop()
        {
            if (!_active) return;
            _active = false;
            YandexGamesSdk.GameStop();
        }
    }
}
