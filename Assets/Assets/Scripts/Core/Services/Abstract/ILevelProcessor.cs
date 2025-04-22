using Core.Services.Models;
using Cysharp.Threading.Tasks;

namespace Core.Services.Abstract
{
    public interface ILevelProcessor
    {
        public ProcessedLevelData[] Process(RemoteLevelData[] rawLevels);
    }
}