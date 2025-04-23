using Cysharp.Threading.Tasks;
using DataBase.Models;

namespace Core.Services.Abstract
{
    public interface ILevelLoader
    {
        public UniTask<RemoteLevelData[]> LoadLevelsAsync();
    }
}