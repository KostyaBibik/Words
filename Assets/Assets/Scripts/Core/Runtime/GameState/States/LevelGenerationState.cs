using System;
using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Core.GameState
{
    public sealed class LevelGenerationState : IGameState
    {
        [Inject] private readonly IGameDataRepository _dataRepository;
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;
        [Inject] private readonly IGameStateMachine _stateMachine;
        
        public async UniTask Enter()
        {
            try
            {
                var levelData = _dataRepository.CurrentLevel;

                await _gameplayPresenter.Initialize(levelData);
                
                await _stateMachine.SwitchState<GameplayState>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Level generation failed: {e}");
                await _stateMachine.SwitchState<MainMenuState>();
            }
        }

        public UniTask Exit() => 
            UniTask.CompletedTask;
    }
}