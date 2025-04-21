using Cysharp.Threading.Tasks;
using UI.Gameplay;
using UnityEngine;
using Zenject;

namespace Core.GameState.States
{
    public class GameplayState : IGameState
    {
        [Inject] private readonly UIGameplayPresenter _gameplayPresenter;
        
        public async UniTask Enter()
        {
            _gameplayPresenter.Show();
             
             await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            _gameplayPresenter.Hide();
            
            await UniTask.CompletedTask;
        }
    }
}