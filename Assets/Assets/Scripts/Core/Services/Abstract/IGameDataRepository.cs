using System.Collections.Generic;
using Core.Services.Models;

namespace Core.Services.Abstract
{
    public interface IGameDataRepository
    {
        IReadOnlyList<ProcessedLevelData> LoadedLevels { get; }
        ProcessedLevelData CurrentProcessedLevel { get; }
        void SetLevels(ProcessedLevelData[] levels);
        void SetCurrentLevel(int index);
    }
}