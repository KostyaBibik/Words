using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.GameState
{
    public sealed class GameStateMachine : IGameStateMachine
    {
        private IGameState _currentState;
        
        private readonly DiContainer _container;

        public GameStateMachine(DiContainer container) => 
            _container = container;

        public async UniTask SwitchState<T>()
            where T : IGameState
        {
            await (_currentState?.Exit() ?? UniTask.CompletedTask);
        
            _currentState = _container.Resolve<T>();
            
            await _currentState.Enter();
        }
    }
}