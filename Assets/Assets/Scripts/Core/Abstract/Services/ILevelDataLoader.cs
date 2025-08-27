using Cysharp.Threading.Tasks;
using DataBase.Models;

namespace Core.Services.Abstract
{
    public interface ILevelDataLoader
    {
        public UniTask<RemoteLevelData[]> LoadLevels();
        public int LoadLevelProgress();
    }
}