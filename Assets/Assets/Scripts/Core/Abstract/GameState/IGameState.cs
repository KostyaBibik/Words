using Cysharp.Threading.Tasks;

namespace Core.GameState
{
    public interface IGameState
    {
        public UniTask Enter();
        public UniTask Exit();
    }
}