using DataBase.Models;

namespace Core.Services.Abstract
{
    public interface ILevelProcessor
    {
        public ProcessedLevelData[] Process(RemoteLevelData[] rawLevels);
    }
}