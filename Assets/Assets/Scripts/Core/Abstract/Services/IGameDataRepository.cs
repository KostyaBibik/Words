using DataBase.Models;

namespace Core.Services
{
    public interface IGameDataRepository
    {
        public ProcessedLevelData CurrentLevel { get; }
        public void SetData(ProcessedLevelData[] levels, int progressId);
        public void IncreaseLevel();
    }
}