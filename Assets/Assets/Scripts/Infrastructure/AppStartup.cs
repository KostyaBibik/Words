using System;
using Core.GameState;
using Core.Services;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using Scripts.Enums;
using UI.ErrorLoading;
using UI.Flow;
using UI.Gameplay;
using UI.Loading;
using UniRx;
using Zenject;
using Gameplay.Utils;
using UI.Gameplay.ClustersPanel;
using UI.Gameplay.Validation;
using UI.Loaders;
using UI.Victory;
using Utils;

namespace Infrastructure
{
    public sealed class AppStartup : IInitializable
    {
        [Inject] private readonly DiContainer _container;
        [Inject] private readonly IUIWindowLoader _uiLoader;
        [Inject] private readonly ILevelDataLoader _levelDataLoader;
        [Inject] private readonly ILevelProcessor _levelProcessor;
        [Inject] private readonly IGameDataRepository _dataRepository;
        [Inject] private readonly IGameStateMachine _gameStateMachine;
        [Inject] private readonly IUIFlowManager _flowManager;
        
        private ReactiveProperty<ELoadPhase> CurrentPhase { get; } = new(ELoadPhase.None);
        
        public void Initialize() =>
            StartLoadProcess().Forget();
        
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
        }

        private async UniTask BindWindowComponents()
        {
            await _container.BindPresenterWithViewFromAddressableAsync<UIGameplayPresenter, UIGameplayView>(UIAddressableKeys.GameplayWindow, _uiLoader);
            await _container.BindPresenterWithViewAsync<UIClustersPanelPresenter, UIClustersPanelView>();
            await _container.BindPresenterWithViewAsync<UIWordGridPresenter, UIWordGridView>();
            await _container.BindPresenterWithViewAsync<UIValidationButtonPresenter, UIValidationButtonView>();
        }
        
        private async UniTask LoadAndProcessLevels()
        {
            UpdateState(ELoadPhase.ConfigsLoading);

            var levels = await _levelDataLoader.LoadLevels();
            if (levels == null || levels.Length == 0)
                throw new Exception("No valid levels found.");

            UpdateState(ELoadPhase.ConfigsProcessing);

            var processedLevels = _levelProcessor.Process(levels);
            if (processedLevels == null || processedLevels.Length == 0)
                throw new Exception("Failed to process levels.");

            var progress = _levelDataLoader.LoadLevelProgress();
            
            _dataRepository.SetData(processedLevels, progress);
            UpdateState(ELoadPhase.Completed);
        }
        
        private void UpdateState(ELoadPhase state) =>
            CurrentPhase.Value = state;
    }
}