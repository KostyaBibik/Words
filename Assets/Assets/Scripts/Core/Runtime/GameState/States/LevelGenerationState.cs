using System;
using Core.Services;
using Cysharp.Threading.Tasks;
using UI.Gameplay;
using UnityEngine;

namespace Core.GameState
{
    public sealed class LevelGenerationState : IGameState
    {
        private readonly IGameDataRepository _dataRepository;
        private readonly UIGameplayPresenter _gameplayPresenter;
        private readonly IGameStateMachine _stateMachine;
        
        public LevelGenerationState(
            IGameDataRepository dataRepository,
            IGameStateMachine stateMachine,
            UIGameplayPresenter gameplayPresenter
        )
        {
            _dataRepository = dataRepository;
            _stateMachine = stateMachine;
            _gameplayPresenter = gameplayPresenter;
        }
        
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