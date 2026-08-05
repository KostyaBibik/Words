using System;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Infrastructure.RemoteConfig
{
    // Levels ship bundled in Resources/Levels rather than via Unity Remote Config:
    // Yandex Games' player CSP blocks connect-src to unity.com domains, so
    // UGS Authentication/Remote Config can never succeed inside the Yandex sandbox.
    public sealed class LevelDataLoader : ILevelDataLoader
    {
        private const string LEVELS_RESOURCE_PATH_RU = "Levels/levels_ru";
        private const string LEVELS_RESOURCE_PATH_EN = "Levels/levels_en";
        private const string ENGLISH_LOCALE_PREFIX = "en";

        private readonly RemoteLevelsContainer _defaultLevels;

        public LevelDataLoader()
        {
            _defaultLevels = new RemoteLevelsContainer
            {
                levels = new[]
                {
                    new RemoteLevelData { id = 0, category = "Резерв", words = new[] {"Резерв"}}
                }
            };
        }

        public UniTask<RemoteLevelData[]> LoadLevels()
        {
            try
            {
                var asset = Resources.Load<TextAsset>(GetLevelsResourcePath());

                if (asset == null)
                    return UniTask.FromResult(_defaultLevels.levels);

                return UniTask.FromResult(JsonUtility.FromJson<RemoteLevelsContainer>(asset.text).levels);
            }
            catch (Exception e)
            {
                Debug.LogError($"Levels load failed: {e.Message}");
                return UniTask.FromResult(_defaultLevels.levels);
            }
        }

        public int LoadLevelProgress()
        {
            try
            {
                return SaveSystem.SaveData.LevelProgress;
            }
            catch (Exception e)
            {
                Debug.LogError($"Levels load failed: {e.Message}");
                return default;
            }
        }

        private static string GetLevelsResourcePath()
        {
            var code = LocalizationSettings.HasSettings
                ? LocalizationSettings.SelectedLocale?.Identifier.Code
                : null;

            return code != null && code.StartsWith(ENGLISH_LOCALE_PREFIX, StringComparison.OrdinalIgnoreCase)
                ? LEVELS_RESOURCE_PATH_EN
                : LEVELS_RESOURCE_PATH_RU;
        }
    }
}