using System;
using System.Collections.Generic;
using Core.Services.Abstract;
using Core.Services.Models;

namespace Core.Services.DataRepository
{
    public class GameDataRepository : IGameDataRepository
    {
        private ProcessedLevelData[] _levels;
        private int _currentLevelIndex;

        public IReadOnlyList<ProcessedLevelData> LoadedLevels => _levels ?? Array.Empty<ProcessedLevelData>();

        public ProcessedLevelData CurrentLevel =>
            (_levels != null && _currentLevelIndex >= 0 && _currentLevelIndex < _levels.Length)
                ? _levels[_currentLevelIndex]
                : null;

        public void SetLevels(ProcessedLevelData[] levels)
        {
            _levels = levels ?? throw new ArgumentNullException(nameof(levels));
            _currentLevelIndex = 0;
        }

        public void SetCurrentLevel(int index)
        {
            if (_levels == null || index < 0 || index >= _levels.Length)
                throw new ArgumentOutOfRangeException(nameof(index));
        
            _currentLevelIndex = index;
        }
    }
}