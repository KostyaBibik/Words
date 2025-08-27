using Assets.Scripts.Core.Abstract.Services;
using UnityEngine;

namespace Assets.Scripts.Infrastructure.RemoteConfig
{
    public sealed class LevelDataSaver : ILevelDataSaver
    {
        private const string PROGRESS_KEY = "level_progress";

        public void SaveProgress(int progress)
        {
            PlayerPrefs.SetInt(PROGRESS_KEY, progress);
            PlayerPrefs.Save();
        }
    }
}