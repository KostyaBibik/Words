using System;
using Core.GameState;
using Core.Services;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using Gameplay.Data.Audio;
using Scripts.Enums;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Loading;
using UniRx;
using Zenject;
using Gameplay.Utils;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Settings;
using UI.Gameplay.Validation;
using UI.Loaders;
using UI.Settings;
using UI.Victory;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Utils;

namespace Infrastructure
{
    public sealed class AppStartup : IInitializable
    {
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly IUIWindowLoader _uiLoader;
        [Inject] private readonly ILevelLoader _levelLoader;
        [Inject] private readonly ILevelProcessor _levelProcessor;
        [Inject] private readonly IGameDataRepository _dataRepository;
        [Inject] private readonly IGameStateMachine _gameStateMachine;
        [Inject] private readonly IUIFlowManager _flowManager;
        [Inject] private readonly IAudioService _audioService;
        
        private ReactiveProperty<ELoadPhase> CurrentPhase { get; } = new(ELoadPhase.None);
        
        public void Initialize() 
        {
            StartLoadProcess().Forget();
        }

        private async UniTaskVoid StartLoadProcess()
        {
            try
            {
                await BindCriticalUI();
           
                _flowManager.ShowLoadingScreen();
                _flowManager.TrackProgress(CurrentPhase);
                
                await BindWindowsFromAddressables();
                
                await BindWindowComponents();
                
                await LoadAndProcessLevels();

                await LoadAudioSettings();
                
                _gameStateMachine.SwitchState<MainMenuState>().Forget();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"Startup failed: {e}");
                UpdateState(ELoadPhase.Failed);
                _flowManager.ShowErrorScreen(e.Message);
            }
        }

        private async UniTask BindCriticalUI()
        {
            var loading = await _container.BindPresenterWithViewFromAddressableAsync<UILoadingPresenter, UILoadingView>(UIAddressableKeys.LoadingWindow, _uiLoader);
            var menu = await _container.BindPresenterWithViewFromAddressableAsync<UIMainMenuPresenter, UIMainMenuView>(UIAddressableKeys.MainMenuWindow, _uiLoader);
            var error = await _container.BindPresenterWithViewFromAddressableAsync<UIErrorLoadingPresenter, UIErrorLoadingView>(UIAddressableKeys.ErrorWindow, _uiLoader);
            
            _flowManager.Init(loading, menu, error);
        }
        
        private async UniTask BindWindowsFromAddressables()
        {
            UpdateState(ELoadPhase.AssetsLoading);
            
            await _container.BindPresenterWithViewFromAddressableAsync<UIVictoryPresenter, UIVictoryView>(_uiLoader, UIAddressableKeys.VictoryWindow);
            await _container.BindPresenterWithViewFromAddressableAsync<UISettingsPanelPresenter, UISettingsPanelView>(_uiLoader, UIAddressableKeys.SettingsWindow);
        }

        private async UniTask BindWindowComponents()
        {
            await _container.BindPresenterWithViewAsync<UISettingsButtonPresenter, UISettingsButtonView>();
            await _container.BindPresenterWithViewFromAddressableAsync<UIGameplayPresenter, UIGameplayView>(UIAddressableKeys.GameplayWindow, _uiLoader);
            await _container.BindPresenterWithViewAsync<UIClustersPanelPresenter, UIClustersPanelView>();
            await _container.BindPresenterWithViewAsync<UIWordGridPresenter, UIWordGridView>();
            await _container.BindPresenterWithViewAsync<UIValidationButtonPresenter, UIValidationButtonView>();
        }
        
        private async UniTask LoadAndProcessLevels()
        {
            UpdateState(ELoadPhase.ConfigsLoading);

            var levels = await _levelLoader.LoadLevelsAsync();
            if (levels == null || levels.Length == 0)
            {
                throw new Exception("No valid levels found.");
            }

            UpdateState(ELoadPhase.ConfigsProcessing);

            var processedLevels = _levelProcessor.Process(levels);
            if (processedLevels == null || processedLevels.Length == 0)
            {
                throw new Exception("Failed to process levels.");
            }

            _dataRepository.SetLevels(processedLevels);
            UpdateState(ELoadPhase.Completed);
        }
        
        private async UniTask LoadAudioSettings()
        {
            UpdateState(ELoadPhase.AudioLoading); 

            var handle = Addressables.LoadAssetAsync<AudioSettings>("Audio Settings");
            await handle.Task;

            if (handle.Status != AsyncOperationStatus.Succeeded)
                throw new Exception("Failed to load AudioSettings");

            var settings = handle.Result;
            _audioService.SetSettings(settings);
            _container.Bind<AudioSettings>().FromInstance(settings).AsSingle(); 
        }
        
        private void UpdateState(ELoadPhase state) =>
            CurrentPhase.Value = state;
    }
}