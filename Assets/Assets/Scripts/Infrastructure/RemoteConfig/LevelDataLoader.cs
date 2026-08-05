using System;
using System.Threading.Tasks;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using DataBase.Models;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Infrastructure.RemoteConfig
{
    public sealed class LevelDataLoader : ILevelDataLoader
    {
        private const string REMOTE_LEVELS_KEY = "levels_json";

        private readonly RemoteLevelsContainer _defaultRemoteLevels;
        
        public LevelDataLoader()
        {
            _defaultRemoteLevels = new RemoteLevelsContainer
            {
                levels = new[]
                {
                    new RemoteLevelData { id = 0, words = new[] {"Резерв"}}
                }
            };
        }

        public async UniTask<RemoteLevelData[]> LoadLevels()
        {
            try
            {
                await InitializeRemoteConfig();

                await FetchConfig();
            
                var jsonData = RemoteConfigService.Instance.appConfig.GetJson(REMOTE_LEVELS_KEY);
                
                if (string.IsNullOrEmpty(jsonData)) 
                    return _defaultRemoteLevels.levels;
            
                return JsonUtility.FromJson<RemoteLevelsContainer>(jsonData).levels;
            }
            catch (Exception e)
            {
                Debug.LogError($"Remote Config failed: {e.Message}");
                return _defaultRemoteLevels.levels;
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
                Debug.LogError($"Remote Config failed: {e.Message}");
                return default;
            }
        }

        private async Task InitializeRemoteConfig()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
                await UnityServices.InitializeAsync();
            
            if (!AuthenticationService.Instance.IsSignedIn)
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        
        private async Task FetchConfig()
        {
            try
            {
                await RemoteConfigService.Instance.FetchConfigsAsync(
                    new UserAttributes(), 
                    new AppAttributes());
            }
            catch (Exception e)
            {
                Debug.LogError($"Remote Config fetch failed: {e.Message}");
            }
        }
        
        private struct UserAttributes {}
        private struct AppAttributes {}
    }
}