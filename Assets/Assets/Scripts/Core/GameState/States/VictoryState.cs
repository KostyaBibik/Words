using Core.Systems.WordContainer;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.GameState.States
{
    public class VictoryState : IGameState
    {
        private readonly WordRepositoryTracker _wordRepositoryTracker;

        public VictoryState(WordRepositoryTracker wordRepositoryTracker)
        {
            _wordRepositoryTracker = wordRepositoryTracker;
        }
        
        public async UniTask Enter()
        {
            Debug.Log("Enter VictoryState");

            var words = _wordRepositoryTracker.GetOrderedWords();

            foreach (var wordData in words)
            {
                Debug.Log($"wordData| orderInWord:{wordData.orderInWord}| value:{wordData.value}| wordGroupIndex:{wordData.wordGroupIndex}");
            }
            
            await UniTask.CompletedTask;
        }

        public UniTask Exit()
        {
            throw new System.NotImplementedException();
        }
    }
}