using Cysharp.Threading.Tasks;

namespace Core.GameState
{
    public interface IGameStateMachine
    {
        public UniTask SwitchState<T>() where T : IGameState;
    }
}