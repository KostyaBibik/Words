using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Services
{
    public interface IValidationService
    {
        public UniTask Validate();
        public IReadOnlyReactiveProperty<bool> ValidationStatus { get; }
        public void Clear();
    }
}