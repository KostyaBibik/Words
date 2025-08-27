using System;
using DataBase.Models;

namespace Core.Services.DataRepository
{
    public sealed class GameDataRepository : IGameDataRepository
    {
        private ProcessedLevelData[] _levels;
        private int _currentLevelIndex;

        public ProcessedLevelData CurrentLevel =>
            _levels != null && _currentLevelIndex >= 0 && _currentLevelIndex < _levels.Length
                ? _levels[_currentLevelIndex]
                : null;

        public void SetData(ProcessedLevelData[] levels, int progressId)
        {
            _levels = levels ?? throw new ArgumentNullException(nameof(levels));
            for (var i = 0; i < _levels.Length; i++)
            {
                if (_levels[i].id == progressId)
                    _currentLevelIndex = i;
            }
        }

        public void IncreaseLevel()
        {
            _currentLevelIndex = _levels.Length - 1 > _currentLevelIndex
                ? _currentLevelIndex + 1
                : 0;
        }
    }
}