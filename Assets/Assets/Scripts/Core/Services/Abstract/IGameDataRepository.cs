using Core.Services.Models;

namespace Core.Services
{
    public interface IGameDataRepository
    {
        public ProcessedLevelData CurrentLevel { get; }
        public void SetLevels(ProcessedLevelData[] levels);
        public void IncreaseLevel();
    }
}