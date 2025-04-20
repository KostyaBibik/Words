using System;
using Core.GameState;
using Core.GameState.States;
using Core.Services.Abstract;
using Cysharp.Threading.Tasks;
using Scripts.Enums;
using UI.Flow;
using UniRx;
using UnityEngine;
using Zenject;

namespace Infrastructure
{
    public sealed class AppStartup : IInitializable
    {
        [Inject] private readonly ILevelLoader _levelLoader;
        [Inject] private readonly ILevelProcessor _levelProcessor;
        [Inject] private readonly IGameDataRepository _dataRepository;
        [Inject] private readonly IUIFlowManager _flowManager;
        [Inject] private readonly IGameStateMachine _gameStateMachine;
        
        public ReactiveProperty<ELoadPhase> CurrentPhase { get; } = new(ELoadPhase.None);
        
        public void Initialize() 
        {
            StartLoadProcess().Forget();
        }

        private async UniTaskVoid StartLoadProcess()
        {
            try
            {
                _flowManager.ShowLoadingScreen();
                UpdateState(ELoadPhase.ConfigsLoading);
                
                var levels = await _levelLoader.LoadLevelsAsync();
                if (levels == null || levels.Length == 0)
                {
                    throw new Exception("No valid levels found.");
                }
                
                
                UpdateState(ELoadPhase.ConfigsProcessing);

                var processedLevels = await _levelProcessor.Process(levels);
                if (processedLevels == null || processedLevels.Length == 0)
                {
                    throw new Exception("Failed to process levels.");
                }
                
                _dataRepository.SetLevels(processedLevels);
                
                UpdateState(ELoadPhase.Completed);

                _gameStateMachine.SwitchState<MainMenuState>().Forget();
            }
            catch (Exception e)
            {
                Debug.LogError($"Startup failed: {e}");
                UpdateState(ELoadPhase.Failed);
                _flowManager.ShowErrorScreen(e.Message);
            }
        }
        
        private void UpdateState(ELoadPhase state)
        {
            Debug.Log($"Current state: {state}");
            
            CurrentPhase.Value = state;
        }
    }
}