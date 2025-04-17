using System;
using System.Threading.Tasks;
using Core.Services.Abstract;
using Core.Services.Models;
using Cysharp.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.RemoteConfig;
using UnityEngine;

namespace Infrastructure.RemoteConfig
{
    public sealed class RemoteLevelLoader : ILevelLoader
    {
        private const string REMOTE_KEY = "levels_json";
        private readonly RemoteLevelsContainer _defaultRemoteLevels;
        
        public RemoteLevelLoader()
        {
            _defaultRemoteLevels = new RemoteLevelsContainer
            {
                levels = new[]
                {
                    new RemoteLevelData { id = 0, words = new[] {"Резерв"}}
                }
            };
        }

        public async UniTask<RemoteLevelData[]> LoadLevelsAsync()
        {
            try
            {
                await InitializeRemoteConfig();

                await FetchConfig();
            
                var json = RemoteConfigService.Instance.appConfig.GetJson(REMOTE_KEY);
                
                if (string.IsNullOrEmpty(json)) 
                    return _defaultRemoteLevels.levels;
            
                return JsonUtility.FromJson<RemoteLevelsContainer>(json).levels;
            }
            catch (Exception e)
            {
                Debug.LogError($"Remote Config failed: {e.Message}");
                return _defaultRemoteLevels.levels;
            }
        }
        
        private async Task InitializeRemoteConfig()
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }
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