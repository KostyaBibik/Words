using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.GameState.States
{
    public class GameplayState : IGameState
    {
        public async UniTask Enter()
        {
             Debug.Log("Enter GameplayState");
             
             await UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }
    }
}