using Core.Services.Models;
using Cysharp.Threading.Tasks;

namespace Core.Services.Abstract
{
    public interface ILevelLoader
    {
        public UniTask<RemoteLevelData[]> LoadLevelsAsync();
    }
}