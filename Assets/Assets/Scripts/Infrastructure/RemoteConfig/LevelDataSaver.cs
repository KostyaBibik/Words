using Assets.Scripts.Core.Abstract.Services;

namespace Assets.Scripts.Infrastructure.RemoteConfig
{
    public sealed class LevelDataSaver : ILevelDataSaver
    {
        public void SaveProgress(int progress)
        {
            SaveSystem.SaveData.LevelProgress = progress;
            SaveSystem.Instance.SaveToStorage();
        }
    }
}