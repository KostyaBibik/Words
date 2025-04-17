using Core.Services.Models;
using Cysharp.Threading.Tasks;

namespace Core.Services.Abstract
{
    public interface ILevelProcessor
    {
        public UniTask<ProcessedLevelData[]> Process(RemoteLevelData[] rawLevels);
    }
}