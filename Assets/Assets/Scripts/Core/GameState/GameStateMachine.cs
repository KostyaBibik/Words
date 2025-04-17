using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.GameState
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly DiContainer _container;
        private IGameState _currentState;

        public GameStateMachine(DiContainer container) 
            => _container = container;

        public async UniTask SwitchState<T>()
            where T : IGameState
        {
            await (_currentState?.Exit() ?? UniTask.CompletedTask);
        
            _currentState = _container.Resolve<T>();
            await _currentState.Enter();
        }
    }
}